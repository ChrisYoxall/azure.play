
# ACA Demo #

Shows how to get a container running on ACA and connecting to several resources


## Azure Container Apps ##

Container Apps run in the context of a 'Container App Environment': https://learn.microsoft.com/en-us/azure/container-apps/environment

Container Apps Environments determine the functionality and billing. Can run either consumption or dedicated profiles. See https://learn.microsoft.com/en-us/azure/container-apps/structure

A Container App Environment must run in a VNet, but by default this is an automatically generated Azure one. See https://learn.microsoft.com/en-us/azure/container-apps/networking

Availability Zones: https://learn.microsoft.com/en-us/azure/reliability/reliability-azure-container-apps



## Terraform ##

Note how the Terraform is configured to create a container application running Nginx. This allows the Terraform to be applied
to create the resources. If we wanted to configure the Terraform to run our application, it would need to be in multiple
parts as first the container registry would need to be created so we can build and push an image, then the container application
could be configured to read that image.

Note that for a live system it's likely that the infrastructure is maintained with Terraform and new image versions
would be deployed using other mechanisms (we will use an Azure CLI command). However, you do need to specify an image
in the Terraform so an easy work-around is to use a publicly available image. The downside is that you do also need
some ignore blocks in the Terraform so it won't overwrite deployment changes.

NOTE: Once run, ensure the values in the demo applications appsettings.json files match what was created
by the Terraform

## Build and push image ##

Build by doing (note that 'yoxalldemoacr' is then name of the container registry specified in the Terraform):

    docker build -t yoxalldemoacr.azurecr.io/aca-demo:0.1.0 .

To run it (and bind the app to 8081 inside the container):

    docker run --name aca-demo --rm -p 8080:8081 -e ASPNETCORE_HTTP_PORTS=8081 yoxalldemoacr.azurecr.io/aca-demo:0.1.0

Without specifying ASPNETCORE_HTTP_PORTS 8080 will be used.

Should now be able to see the forecast at http://localhost:8080/weatherforecast

By default, since ASPNETCORE_ENVIRONMENT is not set inside the container, the app will not bind map the OpenAPI endpoint
(as specified in program.cs). A request to http://localhost:8080/openapi/v1.json will result in a 404 error. If you
pass '-e ASPNETCORE_ENVIRONMENT=Development' as another environment variable to the docker run command above you will
then be able to view the OpenApi document.

Display the 

Login to the ACR:

    az acr login --name yoxalldemoacr

Now push the image to the ACR:

    docker push yoxalldemoacr.azurecr.io/aca-demo:0.1.0


## Health Checks ##

Easy enough to create a health check endpoint from scratch, but they are built into .NET https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks
which means it's a more standardised approach to leverage that functionality.

Good video at https://www.youtube.com/watch?v=p2faw9DCSsY

That video recommends https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks which seem to be commonly used. The
code at least provides examples of how to write health checks even if you don't use the packages themselves.


## Deploy to Container App ##

As described above in the Terraform section, the Terraform code sets up the initial container app to allow the
configuration of infrastructure type concerns such as the container registry, managed identity, etc. Cumbersome to use
Terraform to deploy a new version so will use the Azure CLI for that.

First, dump the existing configuration by doing:

    az containerapp show -g aca-demo-rg -n demo-ca -o yaml

I removed a lot of entries that were related to the running state of the container app, and entries that were null. Next,
make the changes you need, and for this demo I udpdated the image, revisionSuffix, and target port. The result is the
container-template.yaml file.  The schema is described more at https://learn.microsoft.com/en-us/azure/container-apps/azure-resource-manager-api-spec

To make changes, it's probably easier to make them first in the portal, then view the details of the change by doing a 'show' as above, and then
updating the YAML file.

NOTE: Currently hardcoded is the AZURE_CLIENT_ID for the managed ID. This needs to be updated to the current value.

Apply this file by doing:

    az containerapp update -g play-rg -n demo-container-app --yaml ./container-template.yaml

Note that the 'az containerapp update' command can be used to update different parts of the container app. We are supplying a configuration
file to configure the entire app.

There are a lot of other useful AC CLI commands to manage or view the state of container apps.


## Grant RBAC to Azure resources ##

To grant permissions to be able to test locally, in the Azure portal grant your user the appropriate permissions to
the blob storage account and event grid topic:

    Storage: Storage Blob Data Contributor
    Topic: EventGrid Data Sender

Check the Terraform or the managed identity settings in Azure, where you should see those permissions granted.

To view the documents created in CosmosDB, you need to grant data-plane permissions which will not show up under
the RBAC IAM screen in the Portal. To grant using the Azure CLI use something like the following. The 'principal-id'\
below is the Object ID of your user.

    az cosmosdb sql role assignment create \
        --resource-group play-rg \
        --account-name yoxall-demo-cosmos \
        --role-definition-id "/subscriptions/e3768f6b-3a0d-4016-9a03-1b027121d30e/resourceGroups/play-rg/providers/Microsoft.DocumentDB/databaseAccounts/yoxall-demo-cosmos/sqlRoleDefinitions/00000000-0000-0000-0000-000000000002" \
        --principal-id 777cc4ac-cf3b-4a16-8524-634499462b83 \
        --scope "/subscriptions/e3768f6b-3a0d-4016-9a03-1b027121d30e/resourceGroups/play-rg/providers/Microsoft.DocumentDB/databaseAccounts/yoxall-demo-cosmos/dbs/demo-db"
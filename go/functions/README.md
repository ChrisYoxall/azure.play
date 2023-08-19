## Introduction

This example runs a Go application as a custom function handler in Azure. Refer https://learn.microsoft.com/en-us/azure/azure-functions/functions-custom-handlers


## Custom Handler

Initialize an Azure Functions project for custom handlers by doing:

    func init --worker-runtime custom


We will call our application 'app', so make the following changes to the generated host.json file:

- Add the line: "enableForwardingHttpRequest": true
- Set the defaultExecutablePath to be 'app'


Create custom handler for ping endpoint:

    func new -l Custom -t HttpTrigger -n ping -a function

Each custom handler will become a function within the function app when deployed to Azure.



## Go Application

Note how the code uses 'FUNCTIONS_CUSTOMHANDLER_PORT' which holds the port value the functions host will use to call the custom handler.


Build the Go app. It needs to be built to run on the same OS as the function host. Note that I had an issue when deploying to Azure as my
local glibc was ahead of the one on the host. To resolve did a build with cgo disabled:

    GOOS=linux GOARCH=amd64 CGO_ENABLED=0 go build -ldflags="-w -s" -o app


Check to see you can run 'app' directly, then start the function to test locally:

    func start



## Run in Azure

First create the function app. There is an example in the Terraform folders that can be run to create a function app using a Linux host. If using
the portal to create the function app need to specify that the 'Runtime Stack' is custom.

Run the command below to publish to Azure.

    func azure functionapp publish FUNC_APP_NAME_HERE

As the authorisation is set to 'Function' need to pass a code. Can get this from the portal but can also do:

    func azure functionapp list-functions FUNC_APP_NAME_HERE --show-keys


## About

Experiments with Azure Functions

## Notes


To develop locally install the 'Azure Functions Core Tools'. Refer https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local

For local development also install the Azurite storage emulator: https://github.com/azure/azurite

When using C# functions, be aware of 'in-process' and 'isolated worker process' functions. Refer https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-in-process-differences.


## General

Triggers define how a function is invoked and a function must have exactly one trigger. Triggers have associated data, which is often provided as the payload of the function.

Bindings declaratively connecting another resource to the function. Bindings may be input or output bindings or both. Data from bindings is provided to the function as parameters. You can mix and match
bindings as needed. Bindings are optional and a function might have one or multiple input and/or output bindings.

Triggers and bindings let you avoid hardcoding access to other services.


## Deployment

Can deploy using the AZ CLI, for example refer to this Jenkins example: https://learn.microsoft.com/en-us/azure/developer/jenkins/deploy-to-azure-functions

Can also use the 'Azure Functions Core Tools'. For example:

    func azure functionapp publish chris-test-function (from proj folder)

The function will be available at the URL:

    http://FUNC_APP_NAME.azurewebsites.net/api/FUNC_NAME


## Function Authentication

Controlled by AuthorizationLevel. Refer:

- https://learn.microsoft.com/en-us/azure/azure-functions/security-concepts
- https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook-trigger


Levels:

- Anonymous: Anyone can access.
- Key based. There are both host and function keys. Host keys are scoped to function app while function keys are scoped to the function. 
    - Function: Provide either host or function key.
    - Admin: Provide host key.
    - System: Master key.
- User. Not key based. Needs auth token

Supply keys as either a 'code' value in query string or 'x-functions-key' HTTP header:

- curl "https://chris-demo-fa.azurewebsites.net/api/firsthttptrigger?name=bob&code=key_goes_here"
- curl -X POST -H "x-functions-key: key_goes_here" "https://chris-demo-fa.azurewebsites.net/api/firsthttptrigger"


Can get keys for function app by doing: func azure functionapp list-functions [FUNCTION_APP_NAME] --show-keys

For example, a function application called 'chris-demo-fa' with a function called 'azurefule' with the 'AuthorizationLevel' of 'Function' could be called by doing:

    https://chris-demo-fa.azurewebsites.net/api/azurefile?code=XeViiyWC3RkUFRHgwli0cx3Kdu9DC71UzduZycdy6zzsAzFuKHt4qQ==



## RBAC - Managed Identity

Can use managed identity with Azure Functions. Is some Ok documentation such as https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-event-hubs-trigger
(for EventHubs) and https://learn.microsoft.com/en-us/azure/azure-functions/functions-reference?tabs=blob#common-properties-for-identity-based-connections.

Need to assign the managed identity to the function as another step in the deployment.

Refer to the Demo.FunctionApp >> EventHub example. Note how the connection-prefix is set in function, and the app settings has fullyQualifiedNamespace, tennantId, and clientId
 values all with the same connection-prefix and a double underscore.


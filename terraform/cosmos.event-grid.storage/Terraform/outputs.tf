# Output definitions

# Resource Group
output "azurerm_resource_group_name" {
  value       = azurerm_resource_group.rg.name
  description = "The name of the resource group"
}

# Managed Identity
output "managed_identity_name" {
  value       = azurerm_user_assigned_identity.managed_identity.name
  description = "The name of the User Assigned Managed Identity"
}

# Cosmos
output "cosmos_account_name" {
  value       = azurerm_cosmosdb_account.cosmos_account.name
  description = "The name of the Cosmos DB account"
}

output "cosmos_db_name" {
  value       = azurerm_cosmosdb_sql_database.db.name
  description = "The name of the Cosmos DB SQL database"
}

output "cosmos_container_name" {
  value       = azurerm_cosmosdb_sql_container.item_container.name
  description = "The name of the Cosmos DB SQL container"
}

# ACR
output "container_registry_name" {
  value       = azurerm_container_registry.acr.name
  description = "The name of the container registry"
}

# Container App
output "container_app_environment_name" {
  value       = azurerm_container_app_environment.app_env.name
  description = "The name of the container app environment"
}

output "container_app_name" {
  value       = azurerm_container_app.app.name
  description = "The name of the container app"
}
output "container_app_url" {
  value = "https://${azurerm_container_app.app.ingress[0].fqdn}"
}

# Log Analytics outputs
output "log_analytics_workspace_name" {
  value       = azurerm_log_analytics_workspace.logs.name
  description = "The name of the Log Analytics workspace"
}

# Event Grid outputs
output "eventgrid_topic_name" {
  value       = azurerm_eventgrid_topic.topic.name
  description = "The name of the Event Grid topic"
}

# Storage outputs
output "storage_account_name" {
  value       = azurerm_storage_account.storage_account.name
  description = "The name of the storage account"
}

output "storage_container_name" {
  value       = azurerm_storage_container.storage_container.name
  description = "The name of the storage container"
}

# Azure User Assigned Managed Identity
resource "azurerm_user_assigned_identity" "managed_identity" {
  name                = "${var.resource_prefix_name}-managed-identity"
  location            = var.location
  resource_group_name = var.resource_group_name

  tags = {
    environment = "dev"
    managed_by  = "terraform"
  }
}

# Add build in AcrPull role for the managed identity
resource "azurerm_role_assignment" "managed_identity_acr_image_pull" {
  scope                = azurerm_container_registry.acr.id
  role_definition_name = "AcrPull"
  principal_id         = azurerm_user_assigned_identity.managed_identity.principal_id
}

# Add Cosmos DB Data Contributor role for the managed identity
# Refer https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/how-to-grant-data-plane-access
# List available roles:  az cosmosdb sql role definition list --resource-group play-rg --account-name yoxall-demo-cosmos
resource "azurerm_cosmosdb_sql_role_assignment" "managed_identity_cosmos_data_contributor" {
  resource_group_name = var.resource_group_name
  account_name        = azurerm_cosmosdb_account.cosmos_account.name
  role_definition_id  = "${azurerm_cosmosdb_account.cosmos_account.id}/sqlRoleDefinitions/00000000-0000-0000-0000-000000000002"
  principal_id        = azurerm_user_assigned_identity.managed_identity.principal_id
  scope               = "${azurerm_cosmosdb_account.cosmos_account.id}/dbs/${azurerm_cosmosdb_sql_database.db.name}"
}

# Add Storage Blob Data Contributor role for the managed identity
resource "azurerm_role_assignment" "managed_identity_storage_blob_contributor" {
  scope                = azurerm_storage_account.storage_account.id
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = azurerm_user_assigned_identity.managed_identity.principal_id
}

# Add EventGrid Data Sender role for the managed identity
resource "azurerm_role_assignment" "managed_identity_eventgrid_contributor" {
  scope                = azurerm_eventgrid_topic.topic.id
  role_definition_name = "EventGrid Data Sender"
  principal_id         = azurerm_user_assigned_identity.managed_identity.principal_id
}

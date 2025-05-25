
resource "azurerm_cosmosdb_account" "cosmos_account" {
  name                = "yoxall-${var.resource_prefix_name}-cosmos" // needs to be globally unique
  location            = var.location
  resource_group_name = var.resource_group_name
  offer_type          = "Standard"
  kind                = "GlobalDocumentDB"

  # Force Microsoft Entra authentication. Refer https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/how-to-disable-key-based-authentication
  local_authentication_disabled = true

  consistency_policy {
    consistency_level = "Session"
  }

  geo_location {
    location          = var.location
    failover_priority = 0
  }

  // Provisioned (default): You pay for the RU/s you provision whether you use them or not
  // EnableServerless: Serverless option. You only pay for the RU/s you actually consume, up to your specified limits
  capabilities {
    name = "EnableServerless"
  }

  capacity {
    total_throughput_limit = 1000
  }

  tags = {
    environment = "dev"
    managed_by  = "terraform"
  }
}

resource "azurerm_cosmosdb_sql_database" "db" {
  name                = "${var.resource_prefix_name}-db"
  resource_group_name = var.resource_group_name
  account_name        = azurerm_cosmosdb_account.cosmos_account.name
}

resource "azurerm_cosmosdb_sql_container" "item_container" {
  name                = "items"
  resource_group_name = var.resource_group_name
  account_name        = azurerm_cosmosdb_account.cosmos_account.name
  database_name       = azurerm_cosmosdb_sql_database.db.name
  partition_key_paths = ["/id"]
}

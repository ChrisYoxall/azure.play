
resource "azurerm_storage_account" "sa" {
  name                     = var.saname
  resource_group_name      = var.resource_group_name
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "container" {
  name                  = var.containername
  storage_account_name  = azurerm_storage_account.sa.name
  container_access_type = "private"
}
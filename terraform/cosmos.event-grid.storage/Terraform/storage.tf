resource "azurerm_storage_account" "storage_account" {
  name                     = "yoxall${var.resource_prefix_name}storage"
  resource_group_name      = var.resource_group_name
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  access_tier              = "Hot"

  tags = {
    environment = "dev"
    managed_by  = "terraform"
  }
}

resource "azurerm_storage_container" "storage_container" {
  name                  = "test-documents"
  storage_account_id    = azurerm_storage_account.storage_account.id
  container_access_type = "private"
}
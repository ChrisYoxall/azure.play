resource "azurerm_container_registry" "acr" {
  name                = "yoxall${var.resource_prefix_name}acr"
  resource_group_name = var.resource_group_name
  location            = var.location
  sku                 = "Basic"
  admin_enabled       = false

  tags = {
    environment = "dev"
    managed_by  = "terraform"
  }
}
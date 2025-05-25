# Azure Log Analytics Workspace
resource "azurerm_log_analytics_workspace" "logs" {
  name                = "${var.resource_prefix_name}-logs"
  location            = var.location
  resource_group_name = var.resource_group_name
  sku                 = "PerGB2018"
  retention_in_days   = 30

  tags = {
    environment = "dev"
    managed_by  = "terraform"
  }
}
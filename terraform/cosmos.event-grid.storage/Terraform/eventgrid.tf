# Azure Event Grid Topic
resource "azurerm_eventgrid_topic" "topic" {
  name                = "${var.resource_prefix_name}-eventgrid-topic"
  location            = var.location
  resource_group_name = var.resource_group_name

  tags = {
    environment = "dev"
    managed_by  = "terraform"
  }
}
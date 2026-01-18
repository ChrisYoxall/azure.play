terraform {

  required_version = "1.14.3"

  required_providers {

    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>4.57.0"
    }

  }
}

provider "azurerm" {
  subscription_id = var.subscription_id
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
}

# Create service bus, topic & subscription
resource "azurerm_servicebus_namespace" "sb" {
  name                = "yox-demo-servicebus"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "Standard"
}

resource "azurerm_servicebus_topic" "demo_topic" {
  name         = "demo-topic"
  namespace_id = azurerm_servicebus_namespace.sb.id
}

resource "azurerm_servicebus_subscription" "demo_topic_subcription" {
  name               = "demo-subscription"
  topic_id           = azurerm_servicebus_topic.demo_topic.id
  max_delivery_count = 1
}

# Create a managed identity
resource "azurerm_user_assigned_identity" "sb_writer" {
  location            = azurerm_resource_group.rg.location
  name                = "servicebus-writer-identity"
  resource_group_name = azurerm_resource_group.rg.name
}

resource "azurerm_role_assignment" "sb_writer_sender" {
  scope                = azurerm_servicebus_topic.demo_topic.id
  role_definition_name = "Azure Service Bus Data Sender"
  principal_id         = azurerm_user_assigned_identity.sb_writer.principal_id
}
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

resource "azurerm_servicebus_topic_authorization_rule" "demo_topic_writer_rule" {
  name     = "writer-access"
  topic_id = azurerm_servicebus_topic.demo_topic.id
  listen   = false
  send     = true
  manage   = false
}
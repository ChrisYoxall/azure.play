# Creates a function app to run a Go application using a custom handler.

terraform {

  required_version = ">=1.4.0"

  required_providers {

    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>3.54"
    }

  }
}

provider "azurerm" {
  features {}
}

# Create a resource group
resource "azurerm_resource_group" "rg" {
  name     = "chris-function-rg"
  location = "australiaeast"
}

# Create Storage Account required for Function App
resource "azurerm_storage_account" "sa" {
  name                     = "chrisfuncsa"
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

# Create App Service plan
resource "azurerm_service_plan" "asp" {
  name                = "go-functions-asp"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location

  os_type  = "Linux"
  sku_name = "Y1"
}

# Create Azure Function App
resource "azurerm_linux_function_app" "fa" {
  name                = "chrisgofunctions"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location

  service_plan_id = azurerm_service_plan.asp.id

  storage_account_name       = azurerm_storage_account.sa.name
  storage_account_access_key = azurerm_storage_account.sa.primary_access_key

  site_config {}
}
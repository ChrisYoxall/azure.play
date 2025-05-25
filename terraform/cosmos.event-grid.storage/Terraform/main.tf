terraform {

  required_version = "1.12.1"

  required_providers {

    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>4.30.0"
    }

  }
}

provider "azurerm" {
  subscription_id = var.subscription_id
  features {}
}

terraform {

  required_version = ">=1.7.3"

  required_providers {

    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">=3.92.0"
    }

    tls = {
      source  = "hashicorp/tls"
      version = ">=4.0.5"
    }

  }
}

provider "azurerm" {
  features {}
}
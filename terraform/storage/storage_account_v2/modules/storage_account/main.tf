

resource "azurerm_resource_group" "rg" {

  for_each = var.config

  name     = each.value.rgname
  location = each.value.location
}

resource "azurerm_storage_account" "sa" {

  depends_on = [azurerm_resource_group.rg]

  for_each = var.config

  name                     = each.key
  resource_group_name      = each.value.rgname
  location                 = each.value.location
  account_tier             = "Standard"
  account_replication_type = "LRS"

  is_hns_enabled = lookup(each.value, "hierarchical_namespace", false)
  sftp_enabled   = lookup(each.value, "sftp", false)
}

locals {

  containers = flatten([
    for sa_key, sa in var.config : [
      for c in sa.containers : {
        sa_name               = sa_key
        container_name        = c.container_name
        container_access_type = c.container_access_type
      }
    ]
  ])

}

resource "azurerm_storage_container" "container" {

  depends_on = [azurerm_storage_account.sa]

  for_each = { for c in local.containers : c.container_name => c }

  name                  = each.key
  storage_account_name  = each.value.sa_name
  container_access_type = each.value.container_access_type
}

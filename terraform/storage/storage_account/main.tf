# Create Resource Group
module "resource_group" {
  source = "./modules/resource_group"

  rgname   = var.rgname
  location = var.location
}


# Create Storage Account. As this references the name of the resource group this creates an implicit
# dependency on the resource group being created first.
module "storage_account" {
  source = "./modules/storage_account"

  # Access the output from the resource group module here.
  resource_group_name = module.resource_group.rgname

  saname   = var.saname
  location = var.location
}

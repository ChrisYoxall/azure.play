output "primary_access_keys" {
  description = "The primary access key for the storage account"
  value       = module.storage_account.primary_access_keys
  sensitive   = true
}

output "containers_debug" {
  description = "The containers for the storage account. Use to debug."
  value       = module.storage_account.containers
}

output "primary_access_keys" {
  description = "The primary access key for the storage account"
  value       = module.storage_account.primary_access_keys
  sensitive   = true
}
output "primary_key" {
  description = "The primary access key for the storage account"
  value       = module.storage_account.primary_key
  sensitive   = true
}
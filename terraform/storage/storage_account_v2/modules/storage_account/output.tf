output "primary_access_keys" {
  description = "The primary access key for the storage account"
  value       = { for k, v in azurerm_storage_account.sa : k => v.primary_access_key }
  sensitive   = true
}
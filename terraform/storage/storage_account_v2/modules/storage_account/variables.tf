variable "config" {
  description = "Configuration for the storage accounts"
  default = {
    storage_account_one = {
      location = "Australia East"
    },
    storage_account_two = {
      location = "Australia East"
    }
  }
}
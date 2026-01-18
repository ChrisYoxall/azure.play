variable "subscription_id" {
  description = "Azure subscription ID"
  type        = string
  default     = "e3768f6b-3a0d-4016-9a03-1b027121d30e"
  validation {
    condition     = length(var.subscription_id) > 0
    error_message = "Subscription ID can't be empty"
  }
}

variable "location" {
  description = "Location of the Azure resource group"
  type        = string
  default     = "newzealandnorth"
  validation {
    condition     = length(var.location) > 0
    error_message = "Resource group location can't be empty"
  }
}

variable "resource_group_name" {
  description = "Name of the Azure resource group"
  type        = string
  validation {
    condition     = length(var.resource_group_name) > 0
    error_message = "Resource group name can't be empty"
  }
}

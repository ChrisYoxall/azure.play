variable "location" {
  type        = string
  description = "Azure location of resource group"
  default     = "Australia East" # australiaeast
}

variable "rgname" {
  type        = string
  description = "Name of resource group"
  default     = "sa-demo-rg"
}


variable "saname" {
  type        = string
  description = "Name of storage account"
  default     = "yoxalltfdemosa"
}
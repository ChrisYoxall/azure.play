terraform {

  required_providers {

    random = {
      source  = "hashicorp/random"
      version = "3.5.1"
    }


    http = {
      source  = "hashicorp/http"
      version = "3.4.0"
    }

    dns = {
      source  = "hashicorp/dns"
      version = "3.3.2"
    }

  }
}

////* Output an item from a list *////

variable "candy_list" {
  type    = list(string)
  default = ["snickers", "kitkat", "reeces", "m&ms"]
}

output "candy" {
  value = var.candy_list[0]
}

////* Use random provider to generate a name and output it *////

resource "random_pet" "pet" {
  prefix    = "Mr"
  length    = 3
  separator = " "
  keepers = {
    # A new pet name will be gernated with when the id changes
    id = "40"
  }

}

output "pet_name" {
  value = random_pet.pet.id
}


////* Use http provider to get the latest terraform version and output it *////

data "http" "example" {
  url = "https://checkpoint-api.hashicorp.com/v1/check/terraform"

  # Optional request headers
  request_headers = {
    Accept = "application/json"
  }
}

output "http_body" {
  value = data.http.example.response_body
}

output "http_status_code" {
  value = data.http.example.status_code
}

locals {
  http_response_values = jsondecode(data.http.example.response_body)
  version              = lookup(local.http_response_values, "current_version", "")
}

output "latest_terraform_version" {
  value = local.version
}


////* Use dns provider to get the ip address of a domain and output it *////

data "dns_a_record_set" "hacker_news" {
  host = "news.ycombinator.com"
}

output "hacker_news_addrs" {
  value = join(",", data.dns_a_record_set.hacker_news.addrs)
}

////* Use a for expression to pull a distict set of values out of a map *////
variable "disks" {

  default = {

    "jenkins-data-disk" = {
      name                = "disk1"
      resource_group_name = "rg1"
    },
    "nexus-data-disk" = {
      name                = "disk2"
      resource_group_name = "rg2"
    },
    "twistlock-data-disk" = {
      name                = "disk3"
      resource_group_name = "rg2"
    }

  }
}

locals {
  // note that can do 'for key, value' below if you want to reference the key
  disk_rgs = distinct([for value in var.disks : "${value.resource_group_name}"])
}

output "resource_groups" {

  value = local.disk_rgs

}
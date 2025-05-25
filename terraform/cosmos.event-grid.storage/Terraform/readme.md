# Azure Resource Group and CosmosDB Resources Terraform Configuration

This Terraform configuration creates an Azure resource group and CosmosDB resources (account, SQL database, and SQL container) with the following specifications:

## Resource Group
- **Name**: The resource group name is set to `play-rg` in the terraform.tfvars file
- **Location**: Default location is set to `australiaeast`
- **Tags**:
  - environment = "dev"
  - managed_by = "terraform"

## CosmosDB Resources
### CosmosDB Account
- **Name**: The CosmosDB account name is derived from the resource_prefix_name as `yoxall-${resource_prefix_name}-cosmos`
- **Location**: Same as the resource group
- **Offer Type**: Standard
- **Kind**: GlobalDocumentDB
- **Consistency Level**: Session
- **Capabilities**: EnableServerless
- **Total Throughput Limit**: 1000
- **Tags**:
  - environment = "dev"
  - managed_by = "terraform"

### SQL Database
- **Name**: The SQL database name is derived from the resource_prefix_name as `${resource_prefix_name}-db`

### SQL Container
- **Name**: "items"
- **Partition Key Path**: "/id"

## Variables
- `resource_group_name`: Name of the Azure resource group (required, no default value)
- `location`: Location of the Azure resource group (default: "australiaeast")
- `subscription_id`: Azure subscription ID (default: "e3768f6b-3a0d-4016-9a03-1b027121d30e")
- `resource_prefix_name`: The name to use to prefix created resources in Azure (default: "demo")

## Usage
To use this Terraform configuration:

1. Initialize the Terraform working directory:
   ```
   terraform init
   ```

2. Review the planned changes:
   ```
   terraform plan
   ```

3. Apply the changes:
   ```
   terraform apply
   ```

Note: The resource group name must be specified. It is currently set to `play-rg` in the terraform.tfvars file. The CosmosDB account name is derived from the resource_prefix_name variable, which defaults to "demo" if not specified.

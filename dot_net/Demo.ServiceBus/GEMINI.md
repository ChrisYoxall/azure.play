# Project Context: Demo.ServiceBus

## Overview
This project is a demonstration of Azure Service Bus integration using .NET 10 and Terraform. It consists of a .NET Web API application and Infrastructure as Code (IaC) configuration to provision Azure resources.

## Architecture

### Application (`Demo.ServiceBus.Writer`)
- **Type:** .NET Web API
- **Framework:** .NET 10.0 (`net10.0`)
- **Current State:** A message producer for Service Bus.
- **Key Dependencies:** `Microsoft.AspNetCore.OpenApi`

### Infrastructure (`Terraform/`)
- This is a Demo project not for productin use. Must keep Azure costs to a minimum.
- **Tool:** Terraform (v1.14.3+)
- **Provider:** `hashicorp/azurerm` (~>4.30.0)
- **Resources:**
    - Resource Group
    - Azure Service Bus Namespace (`Standard` SKU)
    - Service Bus Topic (`demo-topic`)
    - Service Bus Subscription (`demo-subscription-one`)

### Hosting
- **Platform:** The Demo.ServiceBus.Writer will be deployed to Azure App Service (Linux)
- **Deployment Strategy:** Code-based deployment for ease of use.

## Coding Styles
- **Language:** C# 13 (implied by .NET 10)
- **Style:** Standard .NET coding conventions.
- Plenty of comments given this is a demo project.

## Building and Running

### Application
To run the web API:
```bash
dotnet run --project Demo.ServiceBus.Writer
```

To build the solution:
```bash
dotnet build
```

### Infrastructure
To provision resources (requires Azure authentication, e.g., `az login`):
```bash
cd Terraform
terraform init
terraform apply
```

## Directory Structure
- `Demo.ServiceBus.Writer/`: Source code for the Web API.
- `Terraform/`: Terraform configuration files.
- `Demo.ServiceBus.sln`: .NET Solution file.

## Development Conventions
- **Language:** C# 13 (implied by .NET 10)
- **Style:** Standard .NET coding conventions.
- **IaC:** Terraform best practices (provider pinning, variable usage).

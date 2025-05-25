# Azure Container App Environment
resource "azurerm_container_app_environment" "app_env" {
  name                       = "${var.resource_prefix_name}-container-env"
  location                   = var.location
  resource_group_name        = var.resource_group_name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.logs.id

  workload_profile {
    name                  = "Consumption"
    workload_profile_type = "Consumption"
  }

  tags = {
    environment = "dev"
    managed_by  = "terraform"
  }
}

# Azure Container App
resource "azurerm_container_app" "app" {
  name                         = "${var.resource_prefix_name}-container-app"
  container_app_environment_id = azurerm_container_app_environment.app_env.id
  resource_group_name          = var.resource_group_name
  revision_mode                = "Single"

  workload_profile_name = "Consumption"

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.managed_identity.id]
  }

  registry {
    server   = azurerm_container_registry.acr.login_server
    identity = azurerm_user_assigned_identity.managed_identity.id
  }

  ingress {
    external_enabled           = true
    allow_insecure_connections = false
    target_port                = 80
    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }

  template {
    container {
      name   = "demo-app"
      image  = "docker.io/nginx:1.27.4-alpine"
      cpu    = 0.25
      memory = "0.5Gi"
    }
  }

  # Will manage these outside of Terraform
  lifecycle {
    ignore_changes = [template, ingress]
  }

  tags = {
    environment = "dev"
    managed_by  = "terraform"
  }
}

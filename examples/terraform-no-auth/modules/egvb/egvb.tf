variable "resource_group_name" {
  type = string
}

variable "app_service_plan_name" {
  type = string
}

variable "web_app_name" {
  type = string
}

variable "create_resource" {
  type = bool
}

variable "resource_group_id" {}

resource "random_string" "random" {
  length = 12
  upper = false
  special = false
}

data "http" "deployjson" {
  url = "https://raw.githubusercontent.com/Azure-Samples/eventgrid-viewer-blazor/main/infrastructure/arm/azuredeploy.json"
}

resource "azurerm_template_deployment" "web_app_deploy" {
  count               = var.create_resource ? 1 : 0
  name                = random_string.random.result
  resource_group_name = var.resource_group_name
  deployment_mode     = "Incremental"
  template_body       = data.http.deployjson.body

  parameters = {
    siteName        = var.web_app_name
    hostingPlanName = var.app_service_plan_name
  }

  depends_on          = [var.resource_group_id]
}

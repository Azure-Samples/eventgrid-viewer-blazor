provider "azurerm" {
    version = "=1.36.0"
}

variable "resource_group_name" {
  type = string
}

variable "resource_group_region" {
  type = string
}

variable "web_app_name" {
  type = string
}

variable "app_service_plan_name" {
  type = string
}

module "resourcegroup" {
    source      = "./modules/resource_group"

    name        = var.resource_group_name
    region      = var.resource_group_region
}

module "egvb" {
  source                = "./modules/egvb"
  create_resource       = true
  web_app_name          = var.web_app_name
  app_service_plan_name = var.app_service_plan_name
  resource_group_name   = module.resourcegroup.name
  resource_group_id     = module.resourcegroup.id
}
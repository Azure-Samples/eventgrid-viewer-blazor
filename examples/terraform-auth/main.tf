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

variable "key_vault_name" {
  type = string
}

variable "az_ad_domain" {
  type = string
}

data "azurerm_client_config" "current" {}

module "resourcegroup" {
    source      = "./modules/resource_group"

    name        = var.resource_group_name
    region      = var.resource_group_region
}

module "keyvault" {
    source                = "./modules/key_vault"

    name                  = var.key_vault_name
    resource_group_name   = module.resourcegroup.name
    region                = var.resource_group_region
    tenant_id             = data.azurerm_client_config.current.tenant_id
    resource_group_id     = module.resourcegroup.id
}

module "egvb" {
  source                = "./modules/egvb"
  create_resource       = true
  web_app_name          = var.web_app_name
  app_service_plan_name = var.app_service_plan_name
  resource_group_name   = module.resourcegroup.name
  resource_group_id     = module.resourcegroup.id
  key_vault_id          = module.keyvault.id
  key_vault_name        = var.key_vault_name
  az_ad_domain          = var.az_ad_domain
}
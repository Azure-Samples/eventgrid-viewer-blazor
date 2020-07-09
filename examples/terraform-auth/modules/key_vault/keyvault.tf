variable "resource_group_name" {
  type = string
}

variable "name" {
  type = string
}

variable "region" {
  type = string
}

variable "tenant_id" {
  type = string
}

variable "resource_group_id" {}

resource "azurerm_key_vault" "key_vault" {
  name                        = var.name
  location                    = var.region
  resource_group_name         = var.resource_group_name
  enabled_for_disk_encryption = true
  tenant_id                   = var.tenant_id

  sku_name                    = "standard"

  depends_on                  = [var.resource_group_id]
}

output "id" {
  value = azurerm_key_vault.key_vault.id
}
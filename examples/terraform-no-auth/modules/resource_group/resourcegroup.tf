variable "name" {}

variable "region" {}

resource "azurerm_resource_group" "resource_group" {
  name     = var.name
  location = var.region

  tags = {
    Project = "EventGridViewerBlazor"
  }
}

output "id" {
  value = azurerm_resource_group.resource_group.id
}

output "name" {
  value = var.name
}
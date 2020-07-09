# Overview

Event Subscriptions are made to EventGrid Topics, or to Azure resources.
These are forwarded using SignalR to the browser.

![overview diagram](./images/overview.drawio.svg)

# Configuration

When using Authentication, some settings are kept in the Key Vault.
A policy is applied to the Key Vault to ensure the Managed Identity has GET rights for Secrets.

![configuration diagram](./images/configuration.drawio.svg)

# Authentication

When using Authentication, the user will be challenged to authenticate against Azure Active Directory.

![authentication diagram](./images/authentication.drawio.svg)

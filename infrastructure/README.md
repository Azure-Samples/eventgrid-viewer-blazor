# Overview

The purpose of this document is to provide documentation about the ARM Template & Bash script used to deploy the eventgrid-viewer-blazor application to Azure.  For detailed documentation about the Azure Pipelines, visit [this](azure-pipelines/README.md) document.

## Helpers scripts

- ```utils.sh``` - Script contains utility functions that are shared across all deployment scripts.

## ```azuredeploy.sh Script```

### Purpose

The *azuredeploy* script is used to deploy the eventgrid-viewer-blazor application to Azure directly from a Git repository.  You can run the script locally & it is also called by  the [eventgrid_viewer_blazor_trigger_main Pipeline](azure-pipelines/README.md#eventgrid_viewer_blazor_trigger_main-pipeline)

### ARM Template

The *azuredeploy* script uses the *azuredeploy.json* ARM Template to deploy resources to Azure.

### Azure Resources

- Resource Group
- App Service Plan
- Web App Service
- eventgrid-viewer-blazor code

### Script Parameters

- g - Resource group name - The name of the Azure resource group ie rg-cse-egvb-dev (required)
- s - Site name - The name of the Azure web app ie as-cse-egvb-dev (required)
- p - Hosting plan name - The name of the Azure app service plan ie asp-cse-egvb-dev (required)
- e - Environment - Environment ie dev, test (required)
- l - Location - Azure location ie eastus, westus (required)
- r - Repo url - The git repository where the codebase is located ie ```https://github.com/Azure-Samples/eventgrid-viewer-blazor.git``` (required)
- b - Repo branch - The git repository branch to use ie main (required)
- a - Enable auth - The flag to enable Azure AD authentication.  Default is false (optional)
- k - Keyvault name - The name of the Azure Keyvault to store Azure AD secrets if enabling authentication.  Default is none (optional)
- d - Azure AD domain - The Azure AD Primary Domain if enabling authentication ie ```{youraccount}.onmicrosoft.com```.  Default is none (optional)
- h - Help - Help (optional)

**Sample Usage:**

```bash
    # log into azure & set the default subscription
    az login
    az account set -s {SUBSCRIPTION_NAME_OR_ID}

    # grant execute permissions & run the script
    chmod +x azuredeploy.sh &&
    ./azuredeploy.sh -g rg-cse-egvb-dev -s as-cse-egvb-dev -p asp-cse-egvb-dev -l eastus -r https://github.com/Azure-Samples/eventgrid-viewer-blazor.git -b main
```

# Azure Developer CLI Deployment

This EventGrid Viewer Blazor application can be deployed using the Azure Developer CLI (azd) for a streamlined development and deployment experience.

## Prerequisites

- [Azure Developer CLI](https://aka.ms/azure-dev/install)
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli) (optional, for additional Azure operations)

## Quick Start

1. **Clone and navigate to the repository:**
   ```bash
   git clone https://github.com/Azure-Samples/eventgrid-viewer-blazor.git
   cd eventgrid-viewer-blazor
   ```

2. **Initialize the Azure Developer CLI environment:**
   ```bash
   azd auth login
   azd init
   ```

3. **Deploy the application:**
   ```bash
   azd up
   ```

   This single command will:
   - Provision Azure resources (App Service, Application Insights, Log Analytics)
   - Build and deploy the .NET application
   - Configure all necessary settings

4. **Get the webhook URL:**
   ```bash
   azd env get-values
   ```
   Look for `EVENTGRID_WEBHOOK_URL` in the output.

## Configuration

### Environment Variables

You can customize the deployment by setting environment variables:

```bash
# Set the Azure region
azd env set AZURE_LOCATION eastus2

# Set the App Service SKU
azd env set AZURE_APP_SERVICE_SKU B1

# Enable authentication (requires additional setup)
azd env set AZURE_ENABLE_AUTH true
azd env set AZURE_KEYVAULT_NAME your-keyvault-name
azd env set AZURE_AD_DOMAIN your-domain.onmicrosoft.com
```

### Available Environment Variables

| Variable | Default | Description |
|----------|---------|-------------|
| `AZURE_LOCATION` | `eastus2` | Azure region for deployment |
| `AZURE_APP_SERVICE_SKU` | `F1` | App Service plan SKU |
| `AZURE_ENABLE_AUTH` | `false` | Enable Azure AD authentication |
| `AZURE_KEYVAULT_NAME` | | Key Vault name for auth secrets |
| `AZURE_AD_DOMAIN` | | Azure AD domain for authentication |
| `AZURE_REPO_URL` | Current repo | GitHub repository URL |
| `AZURE_REPO_BRANCH` | `main` | GitHub branch to deploy |

## Authentication Setup

To enable Azure AD authentication:

1. Create an Azure AD app registration
2. Create a Key Vault and store the client ID:
   ```bash
   az keyvault secret set --vault-name <your-keyvault> --name "sct-egvb-azad-client-id" --value "<your-client-id>"
   ```
3. Set the authentication environment variables:
   ```bash
   azd env set AZURE_ENABLE_AUTH true
   azd env set AZURE_KEYVAULT_NAME your-keyvault-name
   azd env set AZURE_AD_DOMAIN your-domain.onmicrosoft.com
   ```
4. Redeploy:
   ```bash
   azd deploy
   ```

## Development Workflow

### Local Development

```bash
# Run the application locally
dotnet run --project src/Blazor.EventGridViewer.ServerApp

# Or use the VS Code task
# Press Ctrl+Shift+P, then "Tasks: Run Task" > "run"
```

### Continuous Deployment

The project includes a GitHub Actions workflow (`.github/workflows/azure-dev.yml`) that automatically deploys on pushes to main. To set this up:

1. Fork the repository
2. Set up GitHub variables for your Azure environment:
   - `AZURE_CLIENT_ID`
   - `AZURE_TENANT_ID` 
   - `AZURE_SUBSCRIPTION_ID`
   - `AZURE_ENV_NAME`
   - `AZURE_LOCATION`

## Common Commands

```bash
# Deploy infrastructure and application
azd up

# Deploy application only (faster for code changes)
azd deploy

# View environment variables and resource URLs
azd env get-values

# View deployment logs
azd monitor

# Clean up all resources
azd down
```

## Resource Naming

Resources are automatically named using the pattern:
- App Service: `app-{env-name}-{random-suffix}`
- App Service Plan: `plan-{env-name}-{random-suffix}`
- Application Insights: `ai-{app-name}-{env-name}`
- Log Analytics: `law-{app-name}-{env-name}`

## Infrastructure

The infrastructure is defined in Bicep templates under the `infra/` directory:

- `main.bicep` - Main template orchestrating all resources
- `modules/monitoring.bicep` - Application Insights and Log Analytics
- `modules/appservice.bicep` - App Service and App Service Plan

## Troubleshooting

### Common Issues

1. **Deployment fails with SKU not available**: Try a different Azure region or SKU:
   ```bash
   azd env set AZURE_LOCATION westus2
   azd env set AZURE_APP_SERVICE_SKU B1
   ```

2. **Resource names already exist**: Change the environment name:
   ```bash
   azd env set AZURE_ENV_NAME myapp-dev-v2
   ```

3. **Authentication not working**: Ensure the Key Vault exists and contains the client ID secret

### Getting Help

- View detailed logs: `azd deploy --debug`
- Check Azure resources: Visit the [Azure Portal](https://portal.azure.com)
- Azure Developer CLI docs: [aka.ms/azure-dev](https://aka.ms/azure-dev)

## Next Steps

After deployment, you can:

1. Configure Event Grid subscriptions to point to: `{your-app-url}/api/eventgrid`
2. View real-time events in the web application
3. Monitor application performance in Application Insights
4. Scale the App Service plan as needed

For more advanced scenarios, see the original [README.md](README.md).
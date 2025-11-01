@description('Location for all resources.')
param location string = resourceGroup().location

@description('The azure web app name.')
param siteName string

@description('The azure web hosting plan name.')
param hostingPlanName string

@allowed([
  'F1'
  'D1'
  'B1'
  'B2'
  'B3'
  'S1'
  'S2'
  'S3'
  'P1'
  'P2'
  'P3'
  'P1V2'
  'P2V2'
  'P3V2'
  'P1V3'
  'P2V3'
  'P3V3'
  'EP1'
  'EP2'
  'EP3'
  'I1'
  'I2'
  'I3'
  'I1V2'
  'I2V2'
  'I3V2'
])
@description('The pricing tier for the hosting plan.')
param sku string

@description('Application Insights connection string.')
param applicationInsightsConnectionString string

@description('Application Insights instrumentation key.')
param applicationInsightsInstrumentationKey string

@description('The URL for the GitHub repository that contains the project to deploy.')
param repoURL string

@description('The branch of the GitHub repository to use.')
param branch string

@description('Deploy source control configuration (set to false if already configured).')
param deploySourceControl bool = false

@description('Tags to apply to all resources.')
param tags object = {}

// Variables

// App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: hostingPlanName
  location: location
  tags: tags
  sku: {
    name: sku
    capacity: sku == 'F1' ? 0 : 1
  }
  properties: {
    reserved: true  // Linux App Service
  }
}

// Web App
resource webApp 'Microsoft.Web/sites@2023-12-01' = {
  name: siteName
  location: location
  tags: union(tags, {
    'azd-service-name': 'web'
  })
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      webSocketsEnabled: true
      linuxFxVersion: 'DOTNETCORE|9.0'
      metadata: [
        {
          name: 'CURRENT_STACK'
          value: 'dotnetcore'
        }
      ]
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: applicationInsightsConnectionString
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: applicationInsightsInstrumentationKey
        }
      ]
    }
  }
}

// Source Control (GitHub deployment) - only deploy if requested
resource sourceControl 'Microsoft.Web/sites/sourcecontrols@2023-12-01' = if (deploySourceControl) {
  name: 'web'
  parent: webApp
  properties: {
    repoUrl: repoURL
    branch: branch
    isManualIntegration: true
  }
}

@description('The Web App instance.')
output webApp object = {
  id: webApp.id
  name: webApp.name
  defaultHostName: webApp.properties.defaultHostName
  url: 'https://${webApp.properties.defaultHostName}'
}

@description('The App Service Plan instance.')
output appServicePlan object = {
  id: appServicePlan.id
  name: appServicePlan.name
}

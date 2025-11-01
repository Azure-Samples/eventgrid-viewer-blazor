@description('Location for all resources.')
param location string = resourceGroup().location

@description('The name prefix for monitoring resources.')
param namePrefix string

@description('Environment name for resource naming.')
param environmentName string

@description('Log Analytics workspace retention in days.')
param retentionInDays int = 30

@description('Application Insights sampling percentage.')
param samplingPercentage int = 5

@description('Application Insights retention in days.')
param appInsightsRetentionInDays int = 90

@description('Tags to apply to all resources.')
param tags object = {}

var logAnalyticsWorkspaceName = 'law-${namePrefix}-${environmentName}'
var applicationInsightsName = 'ai-${namePrefix}-${environmentName}'

// Log Analytics Workspace
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: logAnalyticsWorkspaceName
  location: location
  tags: tags
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: retentionInDays
    features: {
      enableLogAccessUsingOnlyResourcePermissions: true
    }
  }
}

// Application Insights
resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: applicationInsightsName
  location: location
  kind: 'web'
  tags: tags
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
    SamplingPercentage: samplingPercentage
    RetentionInDays: appInsightsRetentionInDays
    DisableIpMasking: false
    DisableLocalAuth: false
  }
}

@description('The Application Insights instance.')
output applicationInsights object = {
  id: applicationInsights.id
  name: applicationInsights.name
  instrumentationKey: applicationInsights.properties.InstrumentationKey
  connectionString: applicationInsights.properties.ConnectionString
}

@description('The Log Analytics workspace instance.')
output logAnalyticsWorkspace object = {
  id: logAnalyticsWorkspace.id
  name: logAnalyticsWorkspace.name
}
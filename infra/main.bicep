targetScope = 'resourceGroup'

metadata name = 'EventGrid Viewer Blazor'
metadata description = 'EventGrid Viewer Blazor application infrastructure'
metadata author = 'Azure Samples'

@minLength(2)
@maxLength(50)
@description('The azure web app name.')
param siteName string

@minLength(2)
@maxLength(50)
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
param sku string = 'F1'

@description('The URL for the GitHub repository that contains the project to deploy.')
param repoURL string = 'https://github.com/Azure-Samples/eventgrid-viewer-blazor.git'

@description('The branch of the GitHub repository to use.')
param branch string = 'main'

@description('Location for all resources.')
param location string = resourceGroup().location

@description('Environment name for resource naming.')
param environmentName string = 'dev'

@description('Deploy source control configuration (set to false if already configured).')
param deploySourceControl bool = false

// Common tags for all resources
var commonTags = {
  'azd-env-name': environmentName
}

// Deploy monitoring resources
module monitoring 'modules/monitoring.bicep' = {
  name: 'monitoring'
  params: {
    location: location
    namePrefix: siteName
    environmentName: environmentName
    tags: commonTags
  }
}

// Deploy app service resources
module appService 'modules/appservice.bicep' = {
  name: 'appservice'
  params: {
    location: location
    siteName: siteName
    hostingPlanName: hostingPlanName
    sku: sku
    applicationInsightsConnectionString: monitoring.outputs.applicationInsights.connectionString
    applicationInsightsInstrumentationKey: monitoring.outputs.applicationInsights.instrumentationKey
    repoURL: repoURL
    branch: branch
    deploySourceControl: deploySourceControl
    tags: commonTags
  }
}

// Outputs
@description('The URL of the deployed web application.')
output webAppUrl string = appService.outputs.webApp.url

@description('The name of the Application Insights instance.')
output applicationInsightsName string = monitoring.outputs.applicationInsights.name

@description('The instrumentation key of the Application Insights instance.')
output applicationInsightsInstrumentationKey string = monitoring.outputs.applicationInsights.instrumentationKey

@description('The connection string of the Application Insights instance.')
output applicationInsightsConnectionString string = monitoring.outputs.applicationInsights.connectionString

@description('The name of the Log Analytics workspace.')
output logAnalyticsWorkspaceName string = monitoring.outputs.logAnalyticsWorkspace.name

@description('The EventGrid webhook endpoint URL.')
output eventGridWebhookUrl string = '${appService.outputs.webApp.url}/api/eventgrid'
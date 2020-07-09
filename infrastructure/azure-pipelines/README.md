# Overview

The purpose of this document is to provide information about the Azure Pipelines used to deploy this project.

## eventgrid_viewer_blazor_pr_master Pipeline

The *eventgrid_viewer_blazor_pr_master* Azure Pipeline is used to build the eventgrid-viewer-blazor application upon a Pull Request.  The pipeline uses the *build-test-report-steps-template* yaml as template to aid in the build process.

### Pipeline Variables

None.

## eventgrid_viewer_blazor_trigger_master Pipeline

The *eventgrid_viewer_blazor_trigger_master* Azure Pipeline is used to deploy the eventgrid-viewer-blazor application to Azure.  The pipeline uses the *deploy-steps-template* yaml as template to aid in the deployment process.

### Pipeline Variables

1. branch - Branch ie master
1. hostingPlanName - Azure App Service Plan name
1. resourceGroup - Azure Resource Group name
1. siteName - Azure Web App name

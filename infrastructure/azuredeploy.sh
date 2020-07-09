#!/bin/bash

#############################################################################################################################################
#- Purpose: Script is used to deploy the EventGrid Viewer Blazor application to Azure.
#- Parameters are:
#- [-g] resource group name - The name of the Azure resource group ie rg-cse-egvb-dev  (required)"
#- [-s] site name - The name of the Azure web app ie as-cse-egvb-dev (required)"
#- [-p] hosting plan name - The name of the Azure app service plan ie asp-cse-egvb-dev (required)"
#- [-l] location - Azure location ie eastus, westus (required)"
#- [-r] repo url - The git repository where the codebase is located ie https://github.com/Azure-Samples/eventgrid-viewer-blazor.git (required)"
#- [-b] repo branch - The git repository branch to use ie master (required)"
#- [-a] enable auth - The flag to enable Azure AD authentication.  Default is false (optional)"
#- [-k] keyvault name - The name of the Azure Keyvault to store Azure AD secrets if enabling authentication.  Default is none (optional)"
#- [-d] azure ad domain - The Azure AD Primary Domain if enabling authentication ie youraccount.onmicrosoft.com.  Default is none (optional)"
#- [-h] help - Help (optional)"
#############################################################################################################################################

set -eu

###############################################################
#- function used to print out script usage
###############################################################
function usage() {
    echo
    echo "Arguments:"
    echo -e "\t-g \t The name of the Azure resource group ie rg-cse-egvb-dev  (required)"
    echo -e "\t-s \t The name of the Azure web app ie as-cse-egvb-dev (required)"
    echo -e "\t-p \t The name of the Azure app service plan ie asp-cse-egvb-dev (required)"
    echo -e "\t-l \t Azure location ie eastus, westus (required)"
    echo -e "\t-r \t The git repository where the codebase is located ie https://github.com/Azure-Samples/eventgrid-viewer-blazor.git (required)"
    echo -e "\t-b \t The git repository branch to use ie master (required)"
    echo -e "\t-a \t The flag to enable Azure AD authentication.  Default is false (optional)"
    echo -e "\t-k \t The name of the Azure Keyvault to store Azure AD secrets if enabling authentication.  Default is none (optional)"
    echo -e "\t-d \t The azure ad domain - The Azure AD Primary Domain if enabling authentication ie youraccount.onmicrosoft.com.  Default is none (optional)"
    echo -e "\t-h \t Help (optional)"
    echo
    echo "Example:"
    echo -e "./azuredeploy.sh -g rg-cse-egvb-dev -s as-cse-egvb-dev -p asp-cse-egvb-dev -l eastus -r https://github.com/Azure-Samples/eventgrid-viewer-blazor.git -b master -k kv-cse-egvb-dev -d microsoft.onmicrosoft.com -a"
}

parent_path=$(
    cd "$(dirname "${BASH_SOURCE[0]}")"
    pwd -P
)

cd "$parent_path"

# add utility.sh script
source helpers/utils.sh

# defaults
enableAuth=false
keyvaultName=none
azAdDomain=none

# Loop, get parameters & remove any spaces from input
while getopts "g:s:p:l:r:b:a:k:d:h" opt; do
    case $opt in
        g)
            # resource group
            resourceGroup=$OPTARG
        ;;
        s)
            # site name
            siteName=$OPTARG
        ;;
        p)
            # hosting plan name
            hostingPlanName=$OPTARG
        ;;
        l)
            # location
            location=$OPTARG
        ;;
        r)
            # repo url
            repoURL=$OPTARG
        ;;
        b)
            # repo branch
            repoBranch=$OPTARG
        ;;
        a)
            # enable auth
            enableAuth=true
        ;;
        k)
            # keyvault name
            keyvaultName=$OPTARG
        ;;
        k)
            # azure directory
            azAdDomain=$OPTARG
        ;;
        :)
          echo "Error: -${OPTARG} requires a value"
          exit 1
        ;;
        *)
          usage
          exit 1
        ;;
    esac
done

# validate parameters
if [[ $# -eq 0 || -z $resourceGroup || -z $siteName || -z $hostingPlanName || -z $location || -z $repoURL || -z $repoBranch ]]; then
    error "Required parameters are missing"
    usage
    exit 1
fi

print "Generate random phrase to be apart of the group deployment name"
group_deployment_name=$(generateRandomPhrase)

#create the azure resource group
 print "Creating resource group $resourceGroup"
 az group create --l $location \
    -n $resourceGroup \
    --tags "Project=EventGridViewerBlazor" \
    -o none

# create the azure deployment
print "Creating the $group_deployment_name deployment group"
az deployment group create -g $resourceGroup \
    -n $group_deployment_name \
    -p siteName=$siteName hostingPlanName=$hostingPlanName repoURL=$repoURL branch=$repoBranch enableAuth=$enableAuth keyvaultName=$keyvaultName azAdDomain=$azAdDomain \
    -f arm/azuredeploy.json \
    -o none
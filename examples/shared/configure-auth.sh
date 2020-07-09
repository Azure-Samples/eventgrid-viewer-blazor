#!/bin/bash

################################################################################################
#- Purpose: Script is used to create an Azure AD App Registration, set Azure
#-          Keyvault secrets & restart the EventGrid Viewer Blazor Azure Web App Service.
#- Parameters are:
#- [-s] azure subscription - The Azure subscription to use (required)"
#- [-g] resource group name - The name of the Azure resource group ie rg-cse-egvb-dev (required)"
#- [-a] site name - The name of the Azure web app ie as-cse-egvb-dev (required)"
#- [-k] keyvault name - The name of the Azure Keyvault to store Azure AD secrets (required) ie kv-cse-egvb-dev"
#- [-h] help - Help (optional)"
################################################################################################

set -eu

###############################################################
#- function used to print out script usage
###############################################################
function usage() {
    echo
    echo "Arguments:"
    echo -e "\t-s \t The Azure subscription to use (required)"
    echo -e "\t-g \t The name of the Azure resource group ie rg-cse-egvb-dev (required)"
    echo -e "\t-a \t The name of the Azure web app ie as-cse-egvb-dev (required)"
    echo -e "\t-k \t The name of the Azure Keyvault to store Azure AD secrets ie kv-cse-egvb-dev (required)"
    echo -e "\t-h \t Help (optional)"
    echo
    echo "Example:"
    echo -e "./configure-auth.sh -s 00000000-1111-2222-3333-444444444444 -g rg-cse-egvb-dev -a as-cse-egvb-dev -k kv-cse-egvb-dev"
}

#######################################################
#- function used to print messages
#######################################################
function print() {
    echo "$1"
}

###############################################################
#- function used to create a random password
###############################################################
function generateRandomPassword() {
    local password=$(openssl rand -base64 16 | colrm 17 | sed 's/$/!/')

    echo "#$password!2"
}

parent_path=$(
    cd "$(dirname "${BASH_SOURCE[0]}")"
    pwd -P
)

cd "$parent_path"

# Loop, get parameters & remove any spaces from input
while getopts "s:g:a:k:h" opt; do
    case $opt in
        s)
            # subscription
            subscription=$OPTARG
        ;;
        g)
            # resource group
            resourceGroup=$OPTARG
        ;;
        a)
            # site name
            siteName=$OPTARG
        ;;
        k)
            # keyvault name
            keyvaultName=$OPTARG
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
if [[ $# -eq 0 || -z $subscription || -z $resourceGroup || -z $siteName || -z $keyvaultName ]]; then
    error "Required parameters are missing"
    usage
    exit 1
fi

# login to azure
az login -o none
az account set -s $subscription

# get configuration values
managedIdentityPrincipalId=$(az webapp identity assign -n $siteName -g $resourceGroup --query 'principalId' -o tsv)
signedInUserPrincipalName=$(az ad signed-in-user show --query 'userPrincipalName' -o tsv)
secret=$(generateRandomPassword)

# create app registration
print "Creating app registration for $siteName"
appId=$(az ad app create --display-name $siteName \
    --identifier-uris https://$siteName.azurewebsites.net \
    --reply-urls https://$siteName.azurewebsites.net/signin-oidc \
    --password $secret \
    --required-resource-accesses @configure-auth-manifest.json \
    --query 'appId' -o tsv)

# set access policies
print "Setting the keyvault access policies"
az keyvault set-policy --name $keyvaultName --upn $signedInUserPrincipalName --secret-permissions set get list delete -o none
az keyvault set-policy --name $keyvaultName --object-id $managedIdentityPrincipalId --secret-permissions set get list -o none

# setting keyvault secrets
print "Setting keyvault secrets"
az keyvault secret set --vault-name $keyvaultName -n 'sct-egvb-azad-client-id' --value $appId -o none

# restart the app
print "Stopping and starting the app"
az webapp stop -n $siteName -g $resourceGroup -o none
az webapp start -n $siteName -g $resourceGroup -o none

# output
print "#######################---OUTPUT---##################################"
print "Signed-In User: $signedInUserPrincipalName"
print "Managed Identity: $managedIdentityPrincipalId"
print "App Registration Secret: $secret"
print "App Registration AppId: $appId"
print "#####################################################################"
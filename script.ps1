$appName = 'bus1hero.IdentityServer'
$appId = '5ea24371-38ef-4a44-93d8-da291059c2ed'
$subscriptionId = 'c9a33ccb-8c38-4f56-9a6d-2e7f4db11a80'
$resourceGroup = 'bus1heroDevEnvSetup'

$json = az ad `
    app `
    create `
    --display-name $appName
$appId = ($json | ConvertFrom-Json).appId
$appId
az ad `
    sp `
    create `
    --id $appId

az ad `
    app `
    credential `
    reset `
    --id $appId
#

az webapp `
    create `
    --name bus1hero-IdentityServer `
    --plan /subscriptions/c9a33ccb-8c38-4f56-9a6d-2e7f4db11a80/resourceGroups/bus1heroDevEnvSetup/providers/Microsoft.Web/serverfarms/ASP-bus1heroDevEnvSetup-aaef `
    --resource-group 'bus1heroDevEnvSetup'


az role `
    assignment `
    create `
    --assignee $appId `
    --role Contributor `
    --scope subscriptions/$subscriptionId/resourceGroups/$resourceGroup

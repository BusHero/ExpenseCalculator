az ad `
    app `
    create `
    --display-name $appName

az ad `
    sp `
    create `
    --id $appId

az ad `
    app `
    credential `
    reset `
    --id $appId

az role `
    assignment `
    create `
    --assignee $appId `
    --role Contributor `
    --scope subscriptions/$subscriptionId/resourceGroups/$resourceGroup

param envName string
param apiEnvironment string
param apiName string
param location string = resourceGroup().location

@secure()
param databaseConnectionString string

var appInsName = 'ins-${apiName}-${envName}'
var aspName = 'plan-${apiName}-${envName}'
var productsApiName = 'api-${apiName}-${envName}'
var keyVaultName = 'kv-chera-productsapi-${envName}'

// Application insights
module appInsights 'appinsights/template.bicep' = {
  name: 'applicationinsights'
  params: {
    name: appInsName
    location: location
  }
}

// App Service Plan
module appServicePlan 'appserviceplan/template.bicep' = {
  name: 'appserviceplan'
  params: {
    planName: aspName
    location: location
  }
}

// Key vault
module keyVault 'keyvault/template.bicep' = {
  name: 'keyvault'
  params: {
    location: location
    kvName: keyVaultName
    // productionSlotId: productAPI.outputs.ProductionObjectId
    // stagingSlotId: productAPI.outputs.StagingObjectId
    readers: {
      items: [
        {
          managedId: productAPI.outputs.ProductionObjectId
        }
        {
          managedId: productAPI.outputs.StagingObjectId
        }
      ]
    }
    secretData: {
      items: [
        {
          name: 'databaseConnectionString'
          value: databaseConnectionString
        }
        {
          name: 'appInsightsKey'
          value: appInsights.outputs.appInsightsKey
        }
      ]
    }
  }
  dependsOn: [
    productAPI
    appInsights
  ]
}

// API
module productAPI 'api/template.bicep' = {
  name: '${apiName}-${envName}'
  params: {
    location: location
    apiEnvironment: apiEnvironment
    apiName: productsApiName
    planName: aspName
    keyVaultName: keyVaultName
  }
  dependsOn: [
    appInsights
    appServicePlan
  ]
}

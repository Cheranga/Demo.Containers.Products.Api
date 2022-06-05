param buildNumber string
param appName string
param environmentName string
param location string = resourceGroup().location
param containerImage string

@secure()
param databaseConnectionString string

var appInsightsName = 'ins-${appName}-${environmentName}'
var aciName = 'aci-${appName}-${environmentName}'
var kvName = 'kv-ccproductsapi-${environmentName}'

module appInsights 'appinsights/template.bicep' = {
  name: '${buildNumber}-appinsights'
  params: {
    location: location
    name: appInsightsName
  }
}

module keyVault 'keyvault/template.bicep' = {
  name: '${buildNumber}-key-vault'  
  params: {
    location:location
    kvName: kvName
    readers: {
      items:[
        {
          managedId: containerInstance.outputs.managedId
        }
      ]
    }
    secretData: {
      items: [
        {
          name: 'DatabaseConfig--ConnectionString'
          value: databaseConnectionString
        }
        {
          name: 'ApplicationInsights--InstrumentationKey'
          value: appInsights.outputs.appInsightsKey
        }
      ]
    }
  }
  dependsOn:[
    appInsights
    containerInstance
  ]
}


module containerInstance 'aci/template.bicep' = {
  name: '${buildNumber}-container-instance'
  params: {
    location: location
    name: aciName
    dnsName: '${appName}-${environmentName}'
    image: containerImage
    keyVaultName: kvName
    appInsightsKey: appInsights.outputs.appInsightsKey
  }
  dependsOn: [
    appInsights
  ]
}

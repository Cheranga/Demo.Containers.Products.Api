param buildNumber string
param appName string
param environmentName string
param location string = resourceGroup().location
param containerImage string

@secure()
param databaseConnectionString string

var appInsightsName = 'ins-${appName}-${environmentName}'
var aciName = 'aci-${appName}-${environmentName}'

module appInsights 'appinsights/template.bicep' = {
  name: '${buildNumber}-appinsights'
  params: {
    location: location
    name: appInsightsName
  }
}

module containerInstance 'aci/template.bicep' = {
  name: '${buildNumber}-container-instance'
  params: {
    location: location
    name: aciName
    dnsName: '${appName}-${environmentName}'
    image: containerImage
    databaseConnectionString: databaseConnectionString
    appInsightsKey: appInsights.outputs.appInsightsKey
  }
  dependsOn: [
    appInsights
  ]
}

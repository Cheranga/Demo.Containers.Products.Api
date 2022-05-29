param envName string
param apiEnvironment string
param apiName string
param location string = resourceGroup().location

var appInsName = 'ins-${apiName}-${envName}'
var aspName = 'plan-${apiName}-${envName}'
var productsApiName = 'api-${apiName}-${envName}'


// Application insights
module appInsights 'appinsights/template.bicep' = {
  name: 'applicationinsights'
  params: {
    name: appInsName
    location:location
  }
}

// App Service Plan
module appServicePlan 'appserviceplan/template.bicep' = {
  name: 'appserviceplan'
  params: {
    planName: aspName
    location:location
  }
}

// API
module productAPI 'api/template.bicep' = {
  name: '${apiName}-${envName}'
  params: {
    location: location
    apiEnvironment: apiEnvironmentName
    apiName: productsApiName    
    planName: aspName
  }
  dependsOn:[
    appInsights
    appServicePlan
  ]
}
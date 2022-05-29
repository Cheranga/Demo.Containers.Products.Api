param buildNumber string
param envName string
param apiName string
param location string = resourceGroup().location
var appInsName = 'ins-${apiName}-${envName}'


// Application insights
module appInsightsModule 'appinsights/template.bicep' = {
  name: '${buildNumber}-applicationinsights'
  params: {
    name: appInsName
    location:location
  }
}
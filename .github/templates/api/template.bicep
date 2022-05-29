param apiName string
param apiEnvironment string
param location string = resourceGroup().location
param planName string

var fullWebAppUriForProductionSlot = '${apiName}.azurewebsites.net'
var websiteTimeZone = 'AUS Eastern Standard Time'

resource apiName_resource 'Microsoft.Web/sites@2019-08-01' = {
  name: apiName
  location: location
  properties: {    
    serverFarmId: resourceId('Microsoft.Web/serverfarms', planName)    
    hostNameSslStates: [
      {
        name: fullWebAppUriForProductionSlot
        sslState: 'Disabled'
        virtualIP: null
        thumbprint: null
        toUpdate: null
        hostType: 'Standard'
      }
      {
        name: fullWebAppUriForStagingSlot
        sslState: 'Disabled'
        virtualIP: null
        thumbprint: null
        toUpdate: null
        hostType: 'Repository'
      }
    ]
    siteConfig: {
      minTlsVersion: '1.2'
      ftpsState: 'Disabled'      
    }
    httpsOnly: true
    clientCertEnabled: false
    clientCertExclusionPaths: ''
    clientAffinityEnabled: false
  }
  identity: {
    type: 'SystemAssigned'
  }
  tags: {
    'hidden-related:/subscriptions/${subscription().id}/resourcegroups/${resourceGroup().name}/providers/Microsoft.Web/serverfarms/${planName}': 'empty'
  }
}

resource apiName_appsettings 'Microsoft.Web/sites/config@2019-08-01' = {
  parent: apiName_resource
  name: 'appsettings'
  properties: {    
    ASPNETCORE_ENVIRONMENT: apiEnvironment
    DIAGNOSTICS_AZUREBLOBCONTAINERSASURL: ''
    DIAGNOSTICS_AZUREBLOBRETENTIONINDAYS: '90'
    WEBSITE_TIME_ZONE: websiteTimeZone
  }
}

output ProductionObjectId string = apiName_resource.identity.principalId
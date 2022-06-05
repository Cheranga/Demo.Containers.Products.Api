param kvName string
param location string = resourceGroup().location

@secure()
param secretData object

param readers object

resource keyVault 'Microsoft.KeyVault/vaults@2021-06-01-preview' = {
  name: kvName
  location: location
  properties: {
    enabledForDeployment: true
    enabledForTemplateDeployment: true
    enabledForDiskEncryption: true
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    accessPolicies: [for item in readers.items: {
      tenantId: subscription().tenantId
      objectId: item.managedId
      permissions: {
        secrets: [
          'get'
          'list'
        ]
      }
    }]    
  }
}

// Keyvault secrets - storage account connection string
resource keyVaultSecret 'Microsoft.KeyVault/vaults/secrets@2021-06-01-preview' = [for item in secretData.items: {
  name: '${keyVault.name}/${item.name}'
  properties: {
    value: '${item.value}'
  }
}]

output keyVaultUri string = keyVault.properties.vaultUri

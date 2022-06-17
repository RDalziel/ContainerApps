param environmentName string
param logAnalyticsWorkspaceName string
param appInsightsName string
param location string
param storageAccountName string
param storageAccountKey string
param storageShareName string

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' = {
  name: logAnalyticsWorkspaceName
  location: location
  properties: any({
    features: {
      searchVersion: 1
    }
    sku: {
      name: 'Free'
    }
  })
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId:logAnalyticsWorkspace.id
  }
}

resource environment 'Microsoft.App/managedEnvironments@2022-03-01' = {
  name: environmentName
  location: location
  properties: {
    daprAIInstrumentationKey: appInsights.properties.InstrumentationKey
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsWorkspace.properties.customerId
        sharedKey: logAnalyticsWorkspace.listKeys().primarySharedKey
      }
    }
  }
}

resource environment_storage 'Microsoft.App/managedEnvironments/storages@2022-03-01' = {
  name: '${environmentName}-share'
  parent: environment
  properties: {
    azureFile: {
      accountKey: storageAccountKey
      accountName: storageAccountName
      shareName: storageShareName
    }
  }
}


output location string = location
output environmentId string = environment.id

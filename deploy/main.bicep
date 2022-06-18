param location string = resourceGroup().location
param environmentName string = 'containerapps-env-prod'

param minReplicas int = 0

param dotnetImage string 
param dotnetPort int = 5000
var dotnetServiceAppName = 'ingestion-api'

param pythonImage string
param pythonPort int = 8000
var pythonServiceAppName = 'calculation-api'

param isPrivateRegistry bool = true

param containerRegistry string
param containerRegistryUsername string
@secure()
param containerRegistryPassword string = ''

var registryPassword = 'registry-password'

var sharedStorageName = '${environmentName}-share'


resource storage 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: replace('${environmentName}', '-', '')
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
  }
}

module environment 'container-app-env.bicep' = {
  name: '${deployment().name}--environment'
  params: {
    environmentName: environmentName
    location: location
    appInsightsName: '${environmentName}-ai'
    logAnalyticsWorkspaceName: '${environmentName}-la'
    storageAccountName: storage.name
    storageAccountKey: storage.listKeys().keys[0].value
    storageShareName: 'fileshare'
  }
}

module pythonService 'container-app-http.bicep' = {
  name: '${deployment().name}--${pythonServiceAppName}'
  dependsOn: [
    environment
  ]
  params: {
    enableIngress: true
    isExternalIngress: true
    location: location
    environmentName: environmentName
    containerAppName: pythonServiceAppName
    containerImage: pythonImage
    containerPort: pythonPort
    isPrivateRegistry: isPrivateRegistry 
    minReplicas: minReplicas
    containerRegistry: containerRegistry
    registryPassword: registryPassword
    containerRegistryUsername: containerRegistryUsername
    storageShareName: sharedStorageName
    secrets: [
      {
        name: registryPassword
        value: containerRegistryPassword
      }
    ]
  }
}


module dotnetService 'container-app-http.bicep' = {
  name: '${deployment().name}--${dotnetServiceAppName}'
  dependsOn: [
    environment
    pythonService
  ]
  params: {
    enableIngress: true
    isExternalIngress: true
    location: location
    environmentName: environmentName
    containerAppName: dotnetServiceAppName
    containerImage: dotnetImage
    containerPort: dotnetPort
    isPrivateRegistry: isPrivateRegistry
    minReplicas: minReplicas
    containerRegistry: containerRegistry
    registryPassword: registryPassword
    containerRegistryUsername: containerRegistryUsername
    storageShareName: sharedStorageName
    env:[
      {
       name: 'pythonServiceEndpoint'
       value: pythonService.outputs.fqdn
      }
      {
        name: 'FileShareBasePath'
        value: '/share'
      }
     ]
    secrets: isPrivateRegistry ? [
      {
        name: registryPassword
        value: containerRegistryPassword
      }
    ] : []
  }
}

output dotnetFqdn string = dotnetService.outputs.fqdn
output pythonFqdn string = pythonService.outputs.fqdn

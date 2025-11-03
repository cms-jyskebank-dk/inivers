param location string = resourceGroup().location
param environmentName string = 'investplatform-env'
param containerAppName string = 'investplatform-api'
param containerImage string

// Log Analytics workspace
resource log 'Microsoft.OperationalInsights/workspaces@2020-08-01' = {
  name: '${containerAppName}-logs'
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
  }
}

// Container Apps managed environment
resource managedEnv 'Microsoft.App/managedEnvironments@2022-03-01' = {
  name: environmentName
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: log.properties.customerId
        sharedKey: listKeys(resourceId('Microsoft.OperationalInsights/workspaces', log.name), '2020-08-01').primarySharedKey
      }
    }
  }
}

// Azure Container Registry
resource acr 'Microsoft.ContainerRegistry/registries@2023-01-01-preview' = {
  name: 'investplatformacr${uniqueString(resourceGroup().id)}'
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {}
}

// Container App
resource containerApp 'Microsoft.App/containerApps@2022-10-01' = {
  name: containerAppName
  location: location
  properties: {
    managedEnvironmentId: managedEnv.id
    configuration: {
      ingress: {
        external: true
        targetPort: 80
        transport: 'Auto'
      }
      registries: [
        {
          server: acr.properties.loginServer
          username: listCredentials(acr.id, '2019-12-01').username
          password: listCredentials(acr.id, '2019-12-01').passwords[0].value
        }
      ]
    }
    template: {
      containers: [
        {
          name: 'api'
          image: containerImage
          resources: {
            cpu: json('0.5')
            memory: '1.0Gi'
          }
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 3
      }
    }
  }
}

output containerAppFqdn string = containerApp.properties.configuration.ingress.fqdn
output acrLoginServer string = acr.properties.loginServer

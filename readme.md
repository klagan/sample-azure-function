## Function Application Samples

## Getting Started

The configuration file required by the application should resemble the following:

`local.settings.json`
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "myServiceBusAdmin": "Endpoint=sb://XXXXX"
  },
  "ConnectionStrings": {
    "myServiceBusWrite": "Endpoint=sb://XXXXX",
    "myServiceBusAdmin": "Endpoint=sb://XXXXX"
  }
}
```

`MyFunctionStartup.cs` - Enables dependency injection
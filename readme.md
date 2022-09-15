# AzFunctions settings parser
Small app that replace keyvault settings in local.settings.json by actual keyvault secret

```
cd azkeyvaultparser
dotnet build
dotnet run
```

## Unable to fetch secret?
This tool is built for dev, so I assume you have authenticated to az somehow, for more information please read this https://docs.microsoft.com/en-us/azure/key-vault/secrets/quick-create-net?tabs=azure-cli

# Azure settings parser
Utility that replaces keyvault settings in appsettings.json|socal.settings.json by actual keyvault secrets

## Prerequisites
- install az cli at https://docs.microsoft.com/en-us/azure/key-vault/secrets/quick-create-net?tabs=azure-cli 
- login to azure using `az login`


```
cd cli
dotnet build

# parse azure function settings (local.settings.json)
dotnet run -- -i /path/to/local.settings.json -o output.json [-t fn]

# parse app service settings (appsettings.json)
dotnet run -- -i /path/to/appsettings.json -o output.json -t app
```

### Options
```
$ dotnet run -- --help
This App uses AAD to authenticate, ensure you have already logged in using az cli
cli 1.0.0
Copyright (C) 2022 cli

  -t, --type      Configuration type.
                  fn: Azure function setings (default).
                  app: Azure App Service configuration

  -i, --input     Path to the file

  -o, --output    Path to the output file

  --help          Display this help screen.

  --version       Display version information.
```


# Bookify Exemplar Clean Architecture Blueprint

## Building 🛠️

[Cake](https://cakebuild.net/) is used to deal with code base maintenance tasks. A Cake wrapper in the solution root called `build.ps1` will ensure that the `dotnet tool` for `cake` is setup.

```powershell
# cake as dotnet tool
dotnet cake --target=Build

# cake wrapper
.\build.ps1 Build

# cake wrapper with named args
.\build.ps1 -Target Build -Configuration Release
```

### Dev-Certs-Setup

Sets up self-signed development certificates as per the [docs](https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https).

```powershell
.\build.ps1 Dev-Certs-Setup
```

This will create `%userprofile%\.aspnet\https\aspnetapp.pfx` and register it as trusted in the Windows Certificate Hive.

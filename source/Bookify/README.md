# Exemplar Clean Architecture Blueprint

## Building 🛠️

[Cake](https://cakebuild.net/) is used to deal with code base maintenance tasks. A Cake wrapper in the solution root called `build.ps1` will ensure that the `dotnet tool` for `cake` is setup.

```powershell
# cake as dotnet tool
dotnet cake --target=Build

# cake wrapper
.\build.ps1 --target=Build

# cake wrapper with named args
.\build.ps1 --target=Build --configuration=Release

# troubleshooting
.\build.ps1 --target=Run-Api --verbosity=diagnostic
```

### Dev-Certs-Setup

Sets up self-signed development certificates as per the [docs](https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https).

```powershell
.\build.ps1 --target=Dev-Certs-Setup
```

This will create `%userprofile%\.aspnet\https\aspnetapp.pfx` and register it as trusted in the Windows Certificate Hive.

## Running 🚀

Cake targets have been setup for more manual chores.

### API

```powershell
.\build.ps1 --target=Run-Api
```

Interact with [Scalar](https://github.com/scalar/scalar) at <http://localhost:5000/scalar/v1>

### Docker Infrastructure

```sh
.\build.ps1 --target=Infra-Up
```

### EF Core Migrations

```sh
.\build.ps1 --target=Add-Migration --name=InitialSetup --verbosity=diagnostic
.\build.ps1 --target=Migrate --verbosity=diagnostic
```


### Keycloak

<>

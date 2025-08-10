# Exemplar Clean Architecture Blueprint

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

## Running 🚀

### VSCode Debug

C# Dev Kit will auto-generate debug/run configurations for the Bookify.Api project. Set that up and hit F5.

### Cake Target

```powershell
.\build.ps1 Run-Api
```

### Docker Containers

```sh
docker compose up
```

## OpenAPI 🎯

Interact with [Scalar](https://github.com/scalar/scalar) at <http://localhost:5000/scalar/v1>

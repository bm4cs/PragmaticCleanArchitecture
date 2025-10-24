# Exemplar Clean Architecture Blueprint

## Building 🛠️

[Cake](https://cakebuild.net/) is used to deal with code base maintenance tasks. A Cake wrapper in the solution root
called `build.ps1` will ensure that the `dotnet tool` for `cake` is setup.

```powershell
dotnet cake --target=Build

# with named args
dotnet cake --target=Build --configuration=Release

# verbose logging
dotnet cake --target=Run-Api --verbosity=diagnostic
```

### Dev-Certs-Setup

Sets up self-signed development certificates as per
the [docs](https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https).

```powershell
dotnet cake --target=Dev-Certs-Setup
```

This will create `%userprofile%\.aspnet\https\aspnetapp.pfx` and register it as trusted in the Windows Certificate Hive.

## Running 🚀

Cake targets have been setup for more manual chores.

### Docker Infrastructure

Start up PostgreSQL and Keycloak.

```sh
dotnet cake --target=Infra-Up
```

### EF Core Migrations

Setup the DB if needed, with EF migrations.

```sh
dotnet cake --target=Add-Migration --name=InitialSetup --verbosity=diagnostic
dotnet cake --target=Migrate --verbosity=diagnostic
```

### Keycloak

A keycloak container is preconfigured to:

- Import realm from `development/keycloak/bookify-realm-export.json` on bootup
- Expose the admin UI <http://localhost:18080/> login with `admin:admin`

### Debug Bookify.Api

1. Explore API with [Scalar](https://github.com/scalar/scalar) at <http://localhost:5000/scalar/v1>
2. Register an account use the `/users/register` endpoint, see `bookify.http`
3. Obtain access token for user, see `bookify.http`
4. 


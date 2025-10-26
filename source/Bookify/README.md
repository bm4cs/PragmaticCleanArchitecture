# Exemplar Clean Architecture Blueprint

<!-- TOC -->
* [Exemplar Clean Architecture Blueprint](#exemplar-clean-architecture-blueprint)
  * [Codebase Tasks](#codebase-tasks)
    * [Dev-Certs-Setup](#dev-certs-setup)
    * [Docker Infrastructure](#docker-infrastructure)
    * [EF Core Migrations](#ef-core-migrations)
    * [Keycloak](#keycloak)
  * [Debugging](#debugging)
<!-- TOC -->

## Codebase Tasks

[Cake](https://cakebuild.net/) is used as a frontend for codebase chores.

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

### Docker Infrastructure

Start up Postgres, Keycloak and Seq infrastructure.

```sh
dotnet cake --target=InfraUp
dotnet cake --target=InfraDown
dotnet cake --target=InfraKill
```

#### Postgres

Initalise the DB schema if needed, with EF migrations.

```sh
dotnet cake --target=AddMigration --name=InitialSetup --verbosity=diagnostic
dotnet cake --target=Migrate --verbosity=diagnostic
```

#### Keycloak

- Imports realm from `development/keycloak/bookify-realm-export.json` on bootup
- Exposes the admin UI <http://localhost:18080/> login with `admin:admin`

#### Seq

- Exposes dashboard UI <http://localhost:8081/> and backend on `5341`

## Debugging

1. Setup run/debug target for `Bookify.Api`
2. Explore API with [Scalar](https://github.com/scalar/scalar) at <http://localhost:5000/scalar/v1>
3. Register an account use the `/users/register` endpoint, see `bookify.http`
4. Obtain access token for user, and explore, see `bookify.http`

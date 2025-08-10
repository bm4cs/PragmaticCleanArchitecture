FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

ENV ASPNETCORE_URLS="http://+:5000;https://+:5001"

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Bookify.Api/Bookify.Api.csproj", "src/Bookify.Api/"]
COPY ["src/Bookify.Application/Bookify.Application.csproj", "src/Bookify.Application/"]
COPY ["src/Bookify.Infrastructure/Bookify.Infrastructure.csproj", "src/Bookify.Infrastructure/"]
COPY ["src/Bookify.Domain/Bookify.Domain.csproj", "src/Bookify.Domain/"]
RUN dotnet restore "src/Bookify.Api/Bookify.Api.csproj"
COPY . .
WORKDIR /src/src/Bookify.Api
RUN dotnet build "Bookify.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Bookify.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookify.Api.dll"]

# docker build . --no-cache --progress=plain -t test

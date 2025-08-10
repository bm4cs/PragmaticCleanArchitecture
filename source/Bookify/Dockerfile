FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 1337

ENV ASPNETCORE_URLS=http://+:1337

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /
COPY ["src/Bookify.Api/Bookify.Api.csproj", "src/Bookify.Api/"]
RUN dotnet restore "src/Bookify.Api/Bookify.Api.csproj"
COPY . .
WORKDIR /src/Bookify.Api
# RUN ls -l /src/Bookify.Api
RUN dotnet build "Bookify.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Bookify.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookify.Api.dll"]

# docker build . --no-cache --progress=plain -t test

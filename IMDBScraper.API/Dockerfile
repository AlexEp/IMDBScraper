FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./IMDBScraper.API.sln .
COPY IMDBScraper.DAL/*.csproj ./IMDBScraper.DAL/
COPY IMDBScraper.Shared/*.csproj ./IMDBScraper.Shared/
COPY IMDBScraper.API/*.csproj ./IMDBScraper.API/

RUN dotnet restore 

# Copy everything else and build
COPY IMDBScraper.DAL/. ./IMDBScraper.DAL/
COPY IMDBScraper.Shared/. ./IMDBScraper.Shared/
COPY IMDBScraper.API/. ./IMDBScraper.API/ 

WORKDIR /app/IMDBScraper.API

# Release to out
RUN dotnet publish /app/IMDBScraper.API.sln  -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build-env /app/IMDBScraper.API/out ./

ENTRYPOINT ["dotnet", "IMDBScraper.API.dll"]
# 1. Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY WeatherForecast.csproj ./WeatherForecast.csproj
RUN dotnet restore WeatherForecast.csproj

COPY . ./
RUN dotnet publish -c Release -o /app

# 2. Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "WeatherForecast.dll"]

# 1. Build bosqichi
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# .csproj faylni copy qilib restore qilamiz
COPY WeatherForecast.csproj ./
RUN dotnet restore WeatherForecast.csproj

# Qolgan fayllarni copy qilamiz
COPY . ./
RUN dotnet publish -c Release -o /app

# 2. Run bosqichi
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

# Botni ishga tushiramiz
ENTRYPOINT ["dotnet", "WeatherForecast.dll"]

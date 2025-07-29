# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# .csproj faylni to‘g‘ri papkasi bilan ko‘chirish
COPY WeatherForecast.csproj ./
RUN dotnet restore

# Endi barcha fayllarni loyihadan nusxalash
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Published fayllarni runtime image ichiga nusxalash
COPY --from=build /app/out .

# Faylni ishga tushirish
ENTRYPOINT ["dotnet", "WeatherForecast.dll"]

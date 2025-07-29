# 1. Rasmiy .NET SDK image bilan build bosqichini boshlaymiz
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 2. Proyekt fayllarini konteynerga nusxalaymiz
COPY . ./

# 3. Loyihani build va publish qilamiz
RUN dotnet publish -c Release -o /app/out

# 4. Yengil runtime imagega o'tamiz
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# 5. Publish qilingan fayllarni ko‘chirib olamiz
COPY --from=build /app/out ./

# 6. Botni ishga tushiramiz (fayl nomini .csproj nomiga moslashtiring!)
ENTRYPOINT ["dotnet", "WeatherForecast.dll"]

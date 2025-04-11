# Базовый образ для запуска (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Образ SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл проекта и выполняем restore (для кэширования зависимостей)
COPY ["Solution/UrlShortener.csproj", "Solution/"]
RUN dotnet restore "./Solution/UrlShortener.csproj"

# Копируем остальное и собираем
COPY . .
WORKDIR "/src/Solution"
RUN dotnet build "./UrlShortener.csproj" -c Release  -o /app/build

# Публикация сборки (Release) в отдельную папку
FROM build AS publish
RUN dotnet publish "./UrlShortener.csproj" -c Release  -o /app/publish /p:UseAppHost=false

# Финальный образ на базе runtime, копируем опубликованные файлы
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "UrlShortener.dll"]

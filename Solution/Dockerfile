# ������� ����� ��� ������� (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# ����� SDK ��� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# �������� ���� ������� � ��������� restore (��� ����������� ������������)
COPY ["Solution/UrlShortener.csproj", "Solution/"]
RUN dotnet restore "./Solution/UrlShortener.csproj"

# �������� ��������� � ��������
COPY . .
WORKDIR "/src/Solution"
RUN dotnet build "./UrlShortener.csproj" -c Release  -o /app/build

# ���������� ������ (Release) � ��������� �����
FROM build AS publish
RUN dotnet publish "./UrlShortener.csproj" -c Release  -o /app/publish /p:UseAppHost=false

# ��������� ����� �� ���� runtime, �������� �������������� �����
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "UrlShortener.dll"]

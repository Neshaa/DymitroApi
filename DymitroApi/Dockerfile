# 1. Faza za build aplikacije
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Kopiraj .csproj i povuci biblioteke (restore)
COPY *.csproj ./
RUN dotnet restore

# Kopiraj sve ostalo i napravi release verziju
COPY . ./
RUN dotnet publish -c Release -o out

# 2. Faza za pokretanje (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Render dinamički dodeljuje PORT, tako da .NET mora da sluša na tom portu
ENV ASPNETCORE_URLS=http://+:10000

# Pokretanje aplikacije (Zameni 'TvojProjekat.dll' sa stvarnim imenom tvog dll fajla)
ENTRYPOINT ["dotnet", "DymitroApi.dll"]
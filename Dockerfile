FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Kopiraj sve .csproj fajlove (čuva layer cache za restore)
COPY Dymitro.Common/Dymitro.Common.csproj Dymitro.Common/
COPY Dymitro.Models/Dymitro.Models.csproj Dymitro.Models/
COPY Dymitro.Contracts/Dymitro.Contracts.csproj Dymitro.Contracts/
COPY Dymitro.DAL.Dapper.Context/Dymitro.DAL.Dapper.Context.csproj Dymitro.DAL.Dapper.Context/
COPY Dymitro.Services/Dymitro.Services.csproj Dymitro.Services/
COPY DymitroApi/DymitroApi.csproj DymitroApi/

RUN dotnet restore DymitroApi/DymitroApi.csproj

# Kopiraj sve i publish
COPY . .
RUN dotnet publish DymitroApi/DymitroApi.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_URLS=http://+:10000

ENTRYPOINT ["dotnet", "DymitroApi.dll"]

FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY MasterDataManager.sln ./
COPY DataLayer/*.csproj ./DataLayer/
COPY MasterDataManager/*.csproj ./MasterDataManager/

RUN dotnet restore
COPY . .
WORKDIR /src/DataLayer
RUN dotnet build -c Release -o /app

WORKDIR /src/MasterDataManager
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MasterDataManager.dll"]
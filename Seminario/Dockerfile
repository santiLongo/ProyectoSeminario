FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./

RUN ls -R

# 🔥 usar la solución completa
RUN dotnet restore ./Seminario/Seminario.sln
RUN dotnet build ./Seminario/Seminario.sln -c Release
RUN dotnet publish ./Seminario/Seminario.sln -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "Seminario.Api.dll"]
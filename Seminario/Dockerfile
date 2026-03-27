FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./

# 🔥 usar la solución completa
RUN dotnet restore Seminario.sln
RUN dotnet publish Seminario.sln -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "Seminario.Api.dll"]
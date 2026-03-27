FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# copiar todo
COPY . ./

# restaurar dependencias
RUN dotnet restore

# publicar
RUN dotnet publish -c Release -o out

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "Seminario.Api.dll"]
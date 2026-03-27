FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./

RUN ls -R

# 🔥 usar la solución completa
RUN dotnet restore $(find . -name "*.sln")
RUN dotnet publish $(find . -name "*.sln") -c Release -o out -v diag

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "Seminario.Api.dll"]
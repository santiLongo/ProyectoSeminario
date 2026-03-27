FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

# Buscar automáticamente el csproj
RUN find . -name "*.csproj"

# Restaurar usando el primero que encuentre
RUN dotnet restore $(find . -name "*.csproj" | head -n 1)

# Publicar usando el mismo
RUN dotnet publish $(find . -name "*.csproj" | head -n 1) -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Ejecutar dinámicamente el dll
CMD ["sh", "-c", "dotnet $(ls *.dll | head -n 1)"]
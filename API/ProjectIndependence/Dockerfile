# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ProjectIndependence.API/*.csproj ProjectIndependence.API/
COPY ProjectIndependence.API.Application/*.csproj ProjectIndependence.API.Application/
COPY ProjectIndependence.API.Core/*.csproj ProjectIndependence.API.Core/
COPY ProjectIndependence.API.Infrastructure/*.csproj ProjectIndependence.API.Infrastructure/

# Restore
RUN dotnet restore ProjectIndependence.API/ProjectIndependence.API.csproj

# Copy everything else
COPY . .

# Build and publish
RUN dotnet publish ProjectIndependence.API/ProjectIndependence.API.csproj -c Release -o /app

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app ./
# COPY certs ./certs

ENTRYPOINT ["dotnet", "ProjectIndependence.API.dll"]

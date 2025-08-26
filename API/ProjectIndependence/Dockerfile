# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ProjectIndependence.sln ./
COPY ProjectIndependence.API/ProjectIndependence.API.csproj ProjectIndependence.API/
COPY ProjectIndependence.API.Core/ProjectIndependence.API.Core.csproj ProjectIndependence.API.Core/
COPY ProjectIndependence.API.Infrastructure/ProjectIndependence.API.Infrastructure.csproj ProjectIndependence.API.Infrastructure/
COPY ProjectIndependence.API.Tests/ProjectIndependence.API.Tests.csproj ProjectIndependence.API.Tests/

# Restore
RUN dotnet restore ProjectIndependence.sln

# Copy everything else
COPY . .

# Build and publish
RUN dotnet publish ProjectIndependence.API/ProjectIndependence.API.csproj -c Release -o /app

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app ./
COPY certs ./certs

ENTRYPOINT ["dotnet", "ProjectIndependence.API.dll"]

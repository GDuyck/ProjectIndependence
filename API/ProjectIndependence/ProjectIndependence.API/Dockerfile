# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files (so restore uses cache effectively)
COPY ProjectIndependence.API/*.csproj ProjectIndependence.API/
COPY ProjectIndependence.API.Application/*.csproj ProjectIndependence.API.Application/
COPY ProjectIndependence.API.Core/*.csproj ProjectIndependence.API.Core/
COPY ProjectIndependence.API.Infrastructure/*.csproj ProjectIndependence.API.Infrastructure/

# Restore dependencies
RUN dotnet restore ProjectIndependence.API/ProjectIndependence.API.csproj

# Copy everything else
COPY . .

# Build + Publish API
RUN dotnet publish ProjectIndependence.API/ProjectIndependence.API.csproj -c Release -o /app/publish 

# Stage 3: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish ./

# Copy certificates (for Kestrel HTTPS)
COPY ./certs ./certs

ENTRYPOINT ["dotnet", "ProjectIndependence.API.dll"]

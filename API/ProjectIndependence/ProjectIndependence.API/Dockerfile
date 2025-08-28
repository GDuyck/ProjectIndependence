# Stage 1: build 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ProjectIndependence.sln ./
COPY ProjectIndependence.API/ProjectIndependence.API.csproj ProjectIndependence.API/
COPY ProjectIndependence.API.Core/ProjectIndependence.API.Core.csproj ProjectIndependence.API.Core/
COPY ProjectIndependence.API.Infrastructure/ProjectIndependence.API.Infrastructure.csproj ProjectIndependence.API.Infrastructure/
COPY ProjectIndependence.API.Tests/ProjectIndependence.API.Tests.csproj ProjectIndependence.API.Tests/

# Restore dependencies
RUN dotnet restore ProjectIndependence.sln

# Copy everything else
COPY . .

# Build
RUN dotnet build ProjectIndependence.sln -c Release --no-restore

# Stage 2: run tests
FROM build AS testrunner
WORKDIR /src
CMD ["dotnet", "test", "ProjectIndependence.API.Tests/ProjectIndependence.API.Tests.csproj", "-c", "Release", "--no-build", "--logger:trx"]

# Stage 3: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Publish API from build stage
COPY --from=build /src/ProjectIndependence.API/bin/Release/net8.0/publish/ ./

COPY ./certs ./certs
ENTRYPOINT ["dotnet", "ProjectIndependence.API.dll"]

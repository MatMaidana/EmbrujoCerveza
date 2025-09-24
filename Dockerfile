# Build stage: restore and publish the ASP.NET Core project
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy solution and project files to enable caching of restore layer
COPY EmbrujoCerveza.sln ./
COPY src/EmbrujoCerveza.Web/EmbrujoCerveza.Web.csproj src/EmbrujoCerveza.Web/

# Restore project dependencies
RUN dotnet restore src/EmbrujoCerveza.Web/EmbrujoCerveza.Web.csproj

# Copy the entire source tree and publish the web app
COPY . .
RUN dotnet publish src/EmbrujoCerveza.Web/EmbrujoCerveza.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage: run the published application on the ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Render expects the service to listen on the port defined in $PORT (defaults to 10000).
# We expose 8080 locally and set ASPNETCORE_URLS to bind to whatever Render assigns at runtime.
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}

COPY --from=build /app/publish .

# Document the port used for local development
EXPOSE 8080

ENTRYPOINT ["dotnet", "EmbrujoCerveza.Web.dll"]

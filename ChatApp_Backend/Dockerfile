# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and restore dependencies
COPY *.sln .
COPY API/*.csproj ./API/
COPY Core/*.csproj ./Core/
COPY Infrastructure/*.csproj ./Infrastructure/
RUN dotnet restore API/API.csproj

# Copy everything else and build

COPY API ./API
COPY Core ./Core
COPY Infrastructure ./Infrastructure

WORKDIR /app/API
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

RUN mkdir -p wwwroot

# Expose port and start the app
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "API.dll"]

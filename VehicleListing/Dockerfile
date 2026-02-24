# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/VehicleListing.Api/VehicleListing.Api.csproj src/VehicleListing.Api/
RUN dotnet restore src/VehicleListing.Api/VehicleListing.Api.csproj

COPY src/ src/
RUN dotnet publish src/VehicleListing.Api/VehicleListing.Api.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "VehicleListing.Api.dll"]

# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution & project files
COPY TariffCalculator.sln ./
COPY src/TariffCalculator.Api.csproj src/

# Restore
RUN dotnet restore src/TariffCalculator.Api.csproj

# Copy everything else
COPY . .

# Publish
RUN dotnet publish src/TariffCalculator.Api.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "TariffCalculator.Api.dll"]
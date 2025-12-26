# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# 1️⃣ Copy NuGet config
# COPY NuGet.config ./

# 2️⃣ Copy ONLY the csproj file (THIS WAS MISSING)
COPY TariffCalculator.Api.csproj ./

# 3️⃣ Restore dependencies
RUN dotnet restore TariffCalculator.Api.csproj \
    --disable-parallel \
    --source https://api.nuget.org/v3/index.json

# 4️⃣ Copy remaining source code
COPY . .

# 5️⃣ Build
RUN dotnet build TariffCalculator.Api.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/build

# =========================
# Publish stage
# =========================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish TariffCalculator.Api.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "TariffCalculator.Api.dll"]
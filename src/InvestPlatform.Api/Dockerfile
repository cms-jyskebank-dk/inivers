# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and restore (optimize Docker layer caching)
COPY . .
RUN dotnet restore "src/InvestPlatform.Api/InvestPlatform.Api.csproj"

# Publish
RUN dotnet publish "src/InvestPlatform.Api/InvestPlatform.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InvestPlatform.Api.dll"]
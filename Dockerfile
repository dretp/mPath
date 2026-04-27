# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore dependencies first (better layer caching)
COPY mPath.csproj ./
RUN dotnet restore "mPath.csproj"

# Copy source and publish
COPY . .
RUN dotnet publish "mPath.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Render sets PORT dynamically; app will be configured in env vars on Render.
EXPOSE 10000

ENTRYPOINT ["dotnet", "mPath.dll"]

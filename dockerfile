# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the full solution
COPY . .

# Restore all projects via the solution file
RUN dotnet restore "TheSteward.sln"

# Publish only the startup project
RUN dotnet publish "TheSteward.Web/TheSteward.Web.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "TheSteward.Web.dll"]
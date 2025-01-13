# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8100
EXPOSE 8101


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Company.API/Company.API.csproj", "Company.API/"]
COPY ["Company.Application/Company.Application.csproj", "Company.Application/"]
COPY ["Company.Domain/Company.Domain.csproj", "Company.Domain/"]
COPY ["Company.Infrastructure/Company.Infrastructure.csproj", "Company.Infrastructure/"]
COPY ["Company.Common/Company.Shared.csproj", "Company.Common/"]
RUN dotnet restore "./Company.API/Company.API.csproj"
COPY . .
WORKDIR "/src/Company.API"
RUN dotnet build "./Company.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Company.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Company.API.dll"]
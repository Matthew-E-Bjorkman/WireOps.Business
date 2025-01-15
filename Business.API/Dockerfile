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
COPY ["Business.API/Business.API.csproj", "Business.API/"]
COPY ["Business.Application/Business.Application.csproj", "Business.Application/"]
COPY ["Business.Domain/Business.Domain.csproj", "Business.Domain/"]
COPY ["Business.Infrastructure/Business.Infrastructure.csproj", "Business.Infrastructure/"]
COPY ["Business.Shared/Business.Shared.csproj", "Business.Shared/"]
RUN dotnet restore "./Business.API/Business.API.csproj"
COPY . .
WORKDIR "/src/Business.API"
RUN dotnet build "./Business.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Business.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Business.API.dll"]
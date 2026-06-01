FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["UserService/UserService.API/UserService.API.csproj", "UserService/UserService.API/"]
COPY ["UserService/UserService.Contracts/UserService.Contracts.csproj", "UserService/UserService.Contracts/"]
COPY ["UserService/UserService.Core/UserService.Core.csproj", "UserService/UserService.Core/"]
COPY ["UserService/UserService.Infrastructure/UserService.Infrastructure.csproj", "UserService/UserService.Infrastructure/"]
COPY ["BuildingBlocks/BuildingBlocks.Contracts/BuildingBlocks.Contracts.csproj", "BuildingBlocks/BuildingBlocks.Contracts/"]

RUN dotnet restore "UserService/UserService.API/UserService.API.csproj"

COPY . .

RUN dotnet publish "UserService/UserService.API/UserService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UserService.API.dll"]
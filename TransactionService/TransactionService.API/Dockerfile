FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["TransactionService/TransactionService.API/TransactionService.API.csproj", "TransactionService/TransactionService.API/"]
COPY ["TransactionService/TransactionService.Contracts/TransactionService.Contracts.csproj", "TransactionService/TransactionService.Contracts/"]
COPY ["TransactionService/TransactionService.Core/TransactionService.Core.csproj", "TransactionService/TransactionService.Core/"]
COPY ["TransactionService/TransactionService.Infrastructure/TransactionService.Infrastructure.csproj", "TransactionService/TransactionService.Infrastructure/"]
COPY ["BuildingBlocks/BuildingBlocks.Contracts/BuildingBlocks.Contracts.csproj", "BuildingBlocks/BuildingBlocks.Contracts/"]

RUN dotnet restore "TransactionService/TransactionService.API/TransactionService.API.csproj"

COPY . .

RUN dotnet publish "TransactionService/TransactionService.API/TransactionService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TransactionService.API.dll"]
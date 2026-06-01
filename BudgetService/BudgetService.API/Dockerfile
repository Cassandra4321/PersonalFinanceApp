FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["BudgetService/BudgetService.API/BudgetService.API.csproj", "BudgetService/BudgetService.API/"]
COPY ["BudgetService/BudgetService.Contracts/BudgetService.Contracts.csproj", "BudgetService/BudgetService.Contracts/"]
COPY ["BudgetService/BudgetService.Core/BudgetService.Core.csproj", "BudgetService/BudgetService.Core/"]
COPY ["BudgetService/BudgetService.Infrastructure/BudgetService.Infrastructure.csproj", "BudgetService/BudgetService.Infrastructure/"]
COPY ["BuildingBlocks/BuildingBlocks.Contracts/BuildingBlocks.Contracts.csproj", "BuildingBlocks/BuildingBlocks.Contracts/"]

RUN dotnet restore "BudgetService/BudgetService.API/BudgetService.API.csproj"

COPY . .

RUN dotnet publish "BudgetService/BudgetService.API/BudgetService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BudgetService.API.dll"]
# PersonalFinanceApp
A backend-focused personal finance application built with a microservices architecture, demonstrating event-driven communication, containerization, and clean architecture principles.

## Architecture
The application consists of three independent microservices, each with its own database and responsibility:
- **UserService** — manages user registration and retrieval
- **TransactionService** — handles financial transactions and validates users via HTTP
- **BudgetService** — tracks spending by consuming events from other services

## Tech Stack
- **ASP.NET Core 9** — REST APIs
- **Entity Framework Core** — ORM and database migrations
- **MassTransit + RabbitMQ** — asynchronous event-driven communication between services
- **SQL Server** — relational database, one per service
- **Docker + Docker Compose** — containerization of all services and infrastructure
- **Swagger / OpenAPI** — API documentation

## Architecture Patterns
- **Clean Architecture** — each service is divided into API, Contracts, Core and Infrastructure layers
- **Domain-Driven Design (DDD)** — value objects, entities and domain logic encapsulated in the Core layer
- **Event-Driven Architecture** — services communicate asynchronously via domain events
- **Repository Pattern** — data access abstracted behind interfaces
- **Dependency Injection** — loosely coupled components throughout

## Event Flow
UserService        --> UserCreatedEvent        --> BudgetService (creates Budget)
TransactionService --> TransactionCreatedEvent --> BudgetService (updates TotalSpent)

## Shared Contracts
The `BuildingBlocks.Contracts` library contains shared event classes used across services, ensuring consistent messaging contracts without coupling the services directly.

# Booking Platform

A distributed property booking platform built with .NET 9.0 microservices architecture, featuring event-driven communication and saga orchestration patterns.

## 🎯 Project Purpose

This is a **demo implementation inspired by Booking.com**, created for **learning and research purposes**. The project demonstrates modern software architecture patterns including microservices, event-driven design, and distributed system orchestration. It serves as a practical exploration of enterprise-level .NET development, showcasing how complex booking workflows can be implemented using industry best practices.

**Note**: This is an educational project and not intended for production use.

## Overview

This system manages the complete booking workflow including user authentication, host registration, property management, and booking orchestration through a scalable microservices architecture.

### Key Features
- **Microservices Architecture**: Distributed system with independent, scalable services
- **Event-driven Communication**: Asynchronous messaging via RabbitMQ
- **Saga Pattern**: Complex workflow orchestration with automatic compensation
- **Domain-driven Design**: Clean architecture with well-defined business domains
- **Modern Stack**: .NET 9.0, PostgreSQL, Redis, Next.js

## Architecture

### Services
```
┌─────────────────┐    ┌─────────────────┐
│    Web UI       │    │   API Gateway   │
│   (Next.js)     │───▶│     Service     │
└─────────────────┘    └─────────────────┘
                                │
            ┌───────────────────┼───────────────────┐
            │                   │                   │
    ┌───────▼────────┐ ┌──────▼──────┐    ┌──────▼──────┐
    │ Identity Service│ │Property     │    │Orchestrator │
    │                 │ │Service      │    │Service      │
    └─────────────────┘ └─────────────┘    └─────────────┘
                                                   │
                                          ┌──────▼──────┐
                                          │   Booking   │
                                          │   Service   │
                                          └─────────────┘
```

### Core Services

- **Identity Service**: User management, authentication & authorization
- **Property Service**: Property, rental units, and amenity management
- **Orchestrator Service**: "Become Host" workflow using Saga pattern
- **API Gateway**: Request routing, authentication validation, rate limiting
- **Booking Service**: Booking management (in development)

### Technology Stack

**Backend (.NET 9.0)**:
- ASP.NET Core 9.0 & Entity Framework Core
- MassTransit + RabbitMQ for messaging
- PostgreSQL databases
- Redis for caching
- JWT authentication

**Frontend**:
- Next.js 15.3.4 with React 19
- TailwindCSS & TypeScript

**Infrastructure**:
- Docker & Docker Compose
- Seq for logging
- Scalar for API documentation

## Quick Start

### Prerequisites
- .NET 9.0 SDK
- Docker & Docker Compose
- Node.js 18+

### Setup & Run

1. **Start dependencies**:
```bash
docker-compose up -d identity-db property-db orchestrator-db redis rabbitmq seq
```

2. **Run services**:
```bash
# Build all services
dotnet build Booking.sln

# Run services individually
cd services/IdentityService/Identity.Api && dotnet run
cd services/PropertyService/Property.Api && dotnet run
cd services/OrchestratorService/Orchestrator.Api && dotnet run
cd services/ApiGateway && dotnet run
```

3. **Run Web UI**:
```bash
cd web-ui
npm install && npm run dev
```

### Using VS Code Tasks
- `Ctrl+Shift+P` → "Run Task" → "start-dependencies"
- `Ctrl+Shift+P` → "Run Task" → "build-identity"
- `Ctrl+Shift+P` → "Run Task" → "build-gateway"

## Key Patterns & Design

### Saga Pattern Implementation
The "Become Host" workflow demonstrates orchestration-based saga:

```
[Started] → [CreatingProperty] → [AddingRentalUnit] → [AddingBedroom] → [UpdatingHostProfile] → [Completed]
```

**Features**:
- Stateful workflow management
- Automatic compensation on failures
- Draft management with Redis caching
- Event-driven state transitions

### Domain-driven Design
- **Aggregates**: Property, User, BecomeHostSaga
- **Value Objects**: Price, Location, HouseRule
- **Events**: PropertyCreated, RentalUnitAdded, BedroomAdded

### CQRS with MediatR
- Command/Query separation
- Centralized request handling
- Pipeline behaviors for cross-cutting concerns

## Development

### Database Setup
Each service has its own PostgreSQL database:
- Identity DB: `localhost:5432`
- Property DB: `localhost:5433`  
- Orchestrator DB: `localhost:5434`

### Environment Configuration
Create environment files:
```bash
# .env.local
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=identitydb;Username=trung;Password=123
ConnectionStrings__Redis=localhost:6379
MessageBroker__Host=rabbitmq://localhost
```

### Monitoring
- **RabbitMQ Management**: `http://localhost:15672` (guest/guest)
- **Seq Logging**: `http://localhost:5341`
- **Redis**: `localhost:6379`

## Roadmap

### Current Status
- ✅ User authentication & authorization
- ✅ Property management with complex domain model
- ✅ Saga-based "Become Host" workflow
- ✅ Event-driven inter-service communication
- 🚧 Booking service implementation
- 🚧 Advanced search & filtering
- 🚧 Payment integration

### Future Enhancements
- **Scalability**: Kubernetes deployment, service mesh
- **Observability**: OpenTelemetry, Prometheus metrics
- **Security**: OAuth 2.0, RBAC, API rate limiting
- **Performance**: Database sharding, CDN integration
- **Business**: Advanced analytics, ML recommendations

## Contributing

This project follows clean architecture principles and modern .NET practices. Key areas for contribution:
- Additional saga workflows
- Enhanced monitoring & observability
- Performance optimizations
- Business feature implementations

## License

This project is for educational and portfolio purposes.

# Booking Platform - Tài liệu Kỹ thuật

## 1. Giới thiệu chung và Mục tiêu dự án

### Tổng quan
Booking Platform là một hệ thống đặt phòng/cho thuê tài sản (properties) theo mô hình microservices, xây dựng trên nền tảng .NET 9.0. Hệ thống được thiết kế để quản lý việc đăng ký host, tạo property, đặt phòng và quản lý người dùng.

### Mục tiêu chính
- **Scalability**: Hệ thống phân tán với kiến trúc microservices
- **Event-driven Architecture**: Sử dụng Saga pattern và Message Broker
- **Domain-driven Design**: Áp dụng DDD với Clean Architecture
- **Consistency**: Đảm bảo tính nhất quán dữ liệu qua các services
- **Maintainability**: Code structure rõ ràng, dễ bảo trì và mở rộng

### Chức năng chính
- Quản lý người dùng và xác thực (Identity Service)
- Đăng ký host và tạo property (Property Service) 
- Orchestration workflow để trở thành host (Orchestrator Service)
- Đặt phòng và quản lý booking (Booking Service)
- API Gateway để điều phối requests
- Web UI cho end-users

## 2. Kiến trúc Tổng thể

### 2.1 Kiến trúc Microservices

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
            │                   │                   │
            │                   │          ┌──────▼──────┐
            │                   │          │   Booking   │
            │                   │          │   Service   │
            │                   │          └─────────────┘
            │                   │                   │
    ┌───────▼───────────────────▼───────────────────▼──────┐
    │              Message Broker (RabbitMQ)              │
    └─────────────────────────────────────────────────────┘
```

### 2.2 Database Architecture

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ Identity DB     │    │ Property DB     │    │Orchestrator DB  │
│ (PostgreSQL)    │    │ (PostgreSQL)    │    │ (PostgreSQL)    │
│ Port: 5432      │    │ Port: 5433      │    │ Port: 5434      │
└─────────────────┘    └─────────────────┘    └─────────────────┘

┌─────────────────┐    ┌─────────────────┐
│   Redis Cache   │    │   Seq Logging   │
│   Port: 6379    │    │   Port: 5341    │
└─────────────────┘    └─────────────────┘
```

### 2.3 Communication Patterns

- **Synchronous**: HTTP/REST APIs cho client requests
- **Asynchronous**: Message Broker (RabbitMQ) cho inter-service communication
- **Saga Pattern**: Orchestration-based saga cho workflow phức tạp
- **Event Sourcing**: Events để maintain consistency

## 3. Các Module chính và Flow Logic

### 3.1 Identity Service
**Chức năng**: Quản lý người dùng, xác thực và phân quyền

**Core Components**:
- `User` Entity với ASP.NET Core Identity
- JWT Service cho authentication
- Redis caching cho session management
- MassTransit consumers cho events

**Flow Logic**:
1. User register/login → Generate JWT token
2. Store user session trong Redis
3. Validate token cho subsequent requests
4. Publish user events qua MassTransit

**Key Technologies**:
- ASP.NET Core Identity
- JWT Authentication
- Redis caching
- PostgreSQL database

### 3.2 Property Service
**Chức năng**: Quản lý properties, rental units, amenities

**Domain Model**:
```csharp
Property (Aggregate Root)
├── PropertyType
├── RentalUnit (Abstract)
│   ├── RoomRentalUnit
│   └── EntirePropertyRentalUnit
├── Amenity
├── Bedroom
└── Image
```

**Flow Logic**:
1. Receive `CreateProperty` message
2. Validate business rules
3. Create Property aggregate
4. Store to database
5. Publish `PropertyCreated` event

**Pattern áp dụng**:
- Domain-driven Design
- Repository Pattern
- Value Objects (Price, Location, HouseRule)
- Table Per Hierarchy (TPH) cho RentalUnit

### 3.3 Orchestrator Service 
**Chức năng**: Orchestrate "Become Host" workflow using Saga pattern

**Saga States**:
```
[Started] → [CreatingProperty] → [AddingRentalUnit] → [AddingBedroom] → [UpdatingHostProfile] → [Completed]
```

**BecomeHostSaga Flow**:
1. **Started**: Receive `BecomeHostStarted` event
2. **CreatingProperty**: Send `CreateProperty` command
3. **AddingRentalUnit**: Send `AddRentalUnit` command  
4. **AddingBedroom**: Send `AddBedroom` command
5. **UpdatingHostProfile**: Send `UpdateHostProfile` command
6. **Completed**: Finalize saga

**Error Handling**: Mỗi step có fault handling để rollback nếu cần

**Draft Management**:
- `BecomeHostDraft` lưu toàn bộ data workflow
- Redis caching cho draft data
- Step-by-step progression tracking

#### 3.3.1 BecomeHost Business Flow Logic - Chi tiết

**Tổng quan Flow Nghiệp vụ**:
BecomeHost là một process phức tạp cho phép user chuyển đổi từ guest thành host trên platform. Quá trình này bao gồm việc tạo property, setup rental units, configure bedrooms và update host profile.

**Sơ đồ Tổng quan BecomeHost Flow**:

```
┌─────────────────────────────────────────────────────────────────────────────────────┐
│                           BECOME HOST SAGA WORKFLOW                                │
└─────────────────────────────────────────────────────────────────────────────────────┘

┌────────────────┐    ┌──────────────────┐    ┌──────────────────┐    ┌──────────────────┐
│   User Action  │    │  Draft Created   │    │   Saga Started   │    │  State: Started  │
│  "Become Host" ├───▶│    in Redis      ├───▶│   in Database    ├───▶│  Store HostId    │
└────────────────┘    └──────────────────┘    └──────────────────┘    └──────────┬───────┘
                                                                                   │
                                            ┌──────────────────────────────────────┘
                                            │
┌───────────────────────────────────────────▼─────────────────────────────────────────────┐
│                              STEP 1: CREATE PROPERTY                                   │
└─────────────────────────────────────────────────────────────────────────────────────────┘

┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ Publish Command │    │ Property Service│    │   Validate &    │    │    Publish      │
│  CreateProperty ├───▶│  Receives Msg   ├───▶│ Create Property ├───▶│ PropertyCreated │
└─────────────────┘    └─────────────────┘    └─────────────────┘    └────────┬────────┘
                                                       │                        │
                                               ┌───────▼────────┐              │
                                               │ Validation     │              │
                                               │ - PropertyType │              │
                                               │ - Host Auth    │              │
                                               │ - Location     │              │
                                               │ - HouseRules   │              │
                                               └────────────────┘              │
                                                                              │
                       ┌──────────────────────────────────────────────────────┘
                       │                                  │
                       ▼                                  ▼
        ┌─────────────────────┐                ┌─────────────────────┐
        │   SUCCESS PATH      │                │   FAILURE PATH      │
        │ State: Creating     │                │ Log Error &         │
        │ Property → Adding   │                │ Finalize Saga       │
        │ RentalUnit          │                │ (Rollback)          │
        └──────────┬──────────┘                └─────────────────────┘
                   │
┌──────────────────▼────────────────────────────────────────────────────────────────────────┐
│                           STEP 2: ADD RENTAL UNIT                                         │
└────────────────────────────────────────────────────────────────────────────────────────────┘

┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ Publish Command │    │ Property Service│    │   Determine     │    │    Publish      │
│  AddRentalUnit  ├───▶│  Receives Msg   ├───▶│   Type & Create ├───▶│ RentalUnitAdded │
└─────────────────┘    └─────────────────┘    └─────────────────┘    └────────┬────────┘
                                                       │                        │
                                               ┌───────▼────────┐              │
                                               │ Business Logic │              │
                                               │ ┌─────────────┐│              │
                                               │ │RoomBased:   ││              │
                                               │ │Name,Quantity││              │
                                               │ │SharedBath   ││              │
                                               │ └─────────────┘│              │
                                               │ ┌─────────────┐│              │
                                               │ │EntireProps: ││              │
                                               │ │Size,Bedrooms││              │
                                               │ │Bathrooms    ││              │
                                               │ └─────────────┘│              │
                                               └────────────────┘              │
                                                                              │
                       ┌──────────────────────────────────────────────────────┘
                       │                                  │
                       ▼                                  ▼
        ┌─────────────────────┐                ┌─────────────────────┐
        │   SUCCESS PATH      │                │   FAILURE PATH      │
        │ State: Adding       │                │ Compensation:       │
        │ RentalUnit →        │                │ Delete Property     │
        │ Adding Bedroom      │                │ & Finalize Saga     │
        └──────────┬──────────┘                └─────────────────────┘
                   │
┌──────────────────▼────────────────────────────────────────────────────────────────────────┐
│                            STEP 3: ADD BEDROOM                                            │
└────────────────────────────────────────────────────────────────────────────────────────────┘

┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ Publish Command │    │ Property Service│    │   Configure     │    │    Publish      │
│  AddBedroom     ├───▶│  Receives Msg   ├───▶│  Bedroom Setup  ├───▶│  BedroomAdded   │
└─────────────────┘    └─────────────────┘    └─────────────────┘    └────────┬────────┘
                                                       │                        │
                                               ┌───────▼────────┐              │
                                               │ Bed Config     │              │
                                               │ - SingleBeds   │              │
                                               │ - DoubleBeds   │              │
                                               │ - KingBeds     │              │
                                               │ - SofaBeds     │              │
                                               │ Validation:    │              │
                                               │ Total > 0      │              │
                                               └────────────────┘              │
                                                                              │
                       ┌──────────────────────────────────────────────────────┘
                       │                                  │
                       ▼                                  ▼
        ┌─────────────────────┐                ┌─────────────────────┐
        │   SUCCESS PATH      │                │   FAILURE PATH      │
        │ State: Adding       │                │ Compensation:       │
        │ Bedroom →           │                │ Delete RentalUnit   │
        │ Updating Profile    │                │ Delete Property     │
        └──────────┬──────────┘                └─────────────────────┘
                   │
┌──────────────────▼────────────────────────────────────────────────────────────────────────┐
│                        STEP 4: UPDATE HOST PROFILE                                        │
└────────────────────────────────────────────────────────────────────────────────────────────┘

┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ Publish Command │    │ Identity Service│    │   Update User   │    │    Publish      │
│UpdateHostProfile├───▶│  Receives Msg   ├───▶│   to Host       ├───▶│HostProfileUpdte │
└─────────────────┘    └─────────────────┘    └─────────────────┘    └────────┬────────┘
                                                       │                        │
                                               ┌───────▼────────┐              │
                                               │ Profile Update │              │
                                               │ - Languages    │              │
                                               │ - Bio/Desc     │              │
                                               │ - Contact Pref │              │
                                               │ - Host Status  │              │
                                               │   = Active     │              │
                                               └────────────────┘              │
                                                                              │
                       ┌──────────────────────────────────────────────────────┘
                       │                                  │
                       ▼                                  ▼
        ┌─────────────────────┐                ┌─────────────────────┐
        │   SUCCESS PATH      │                │   FAILURE PATH      │
        │ State: Completed    │                │ Mark Property as    │
        │ Finalize Saga       │                │ "Incomplete" &      │
        │ Clean Draft         │                │ Send Notification   │
        └─────────────────────┘                └─────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────────────────────┐
│                                  FINAL RESULT                                              │
└─────────────────────────────────────────────────────────────────────────────────────────────┘

        ✅ SUCCESS                                    ❌ FAILURE
┌─────────────────────┐                    ┌─────────────────────┐
│ • User = Host       │                    │ • Rollback Actions  │
│ • Property Active   │                    │ • Error Logged      │
│ • RentalUnit Ready  │                    │ • User Notified     │
│ • Bedroom Config    │                    │ • Draft Preserved   │
│ • Host Dashboard    │                    │ • Retry Available   │
└─────────────────────┘                    └─────────────────────┘
```

**Service Interaction Diagram**:

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│   Web UI    │    │ API Gateway │    │Orchestrator │    │   Redis     │
│             │    │             │    │   Service   │    │   Cache     │
└──────┬──────┘    └──────┬──────┘    └──────┬──────┘    └──────┬──────┘
       │                  │                  │                  │
       │ 1. Submit Draft  │                  │                  │
       ├─────────────────▶│                  │                  │
       │                  │ 2. Store Draft  │                  │
       │                  ├─────────────────▶│                  │
       │                  │                  │ 3. Cache Draft   │
       │                  │                  ├─────────────────▶│
       │                  │                  │                  │
       │                  │ 4. Start Saga    │                  │
       │                  ├─────────────────▶│                  │
       │                  │                  │                  │

┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│Orchestrator │    │  RabbitMQ   │    │  Property   │    │ PostgreSQL  │
│   Service   │    │   Broker    │    │   Service   │    │   Database  │
└──────┬──────┘    └──────┬──────┘    └──────┬──────┘    └──────┬──────┘
       │                  │                  │                  │
       │ 5. CreateProperty│                  │                  │
       ├─────────────────▶│                  │                  │
       │                  │ 6. Route Msg     │                  │
       │                  ├─────────────────▶│                  │
       │                  │                  │ 7. Store Entity │
       │                  │                  ├─────────────────▶│
       │                  │ 8. PropertyCreated                  │
       │                  │◀─────────────────┤                  │
       │ 9. Receive Event │                  │                  │
       │◀─────────────────┤                  │                  │

[Similar pattern repeats for AddRentalUnit, AddBedroom, UpdateHostProfile]
```

**Detailed Step-by-Step Flow**:

**Step 1: Initiate BecomeHost Process**
```
User Action: Click "Become a Host" trên UI
↓
Frontend: Submit BecomeHostDraft với initial data
↓
Orchestrator: Create BecomeHostSaga instance
↓
State: [Started] → Store HostId và Draft data
```

**Step 2: Property Creation**
```
Saga Action: Publish CreateProperty command
↓
Property Service: Receive CreateProperty message
├── Validate: PropertyType exists
├── Validate: Host has permission
├── Create: Property aggregate với Location, HouseRules
├── Store: Property entity to database
└── Publish: PropertyCreated event

Success Path:
PropertyCreated → Saga receives event → Store PropertyId → Transition to [AddingRentalUnit]

Failure Path:
CreatePropertyFailed → Log error → Finalize saga (rollback)
```

**Step 3: Rental Unit Setup**
```
Saga Action: Publish AddRentalUnit command với PropertyId
↓
Property Service: Receive AddRentalUnit message
├── Validate: Property exists và belongs to Host
├── Determine: RentalUnit type (Room vs EntireProperty)
├── Create: RentalUnit aggregate
│   ├── RoomRentalUnit: Name, Quantity, SharedBathroom
│   └── EntirePropertyRentalUnit: Size, BedroomsCount, BathroomsCount
├── Set: BasePricePerNight (Amount + Currency)
├── Add: Amenities (if provided)
└── Publish: RentalUnitAdded event

Success Path:
RentalUnitAdded → Saga receives event → Store RentalUnitId → Transition to [AddingBedroom]

Failure Path:
AddRentalUnitFailed → Log error → Finalize saga (rollback)
```

**Step 4: Bedroom Configuration**
```
Saga Action: Publish AddBedroom command với RentalUnitId
↓
Property Service: Receive AddBedroom message
├── Validate: RentalUnit exists
├── Create: Bedroom entity với bed configurations
│   ├── SingleBeds count
│   ├── DoubleBeds count  
│   ├── KingBeds count
│   └── SofaBeds count
├── Associate: Bedroom với RentalUnit
└── Publish: BedroomAdded event

Success Path:
BedroomAdded → Saga receives event → Transition to [UpdatingHostProfile]

Failure Path:
AddBedroomFailed → Log error → Finalize saga (rollback)
```

**Step 5: Host Profile Update**
```
Saga Action: Publish UpdateHostProfile command với HostId
↓
Identity Service: Receive UpdateHostProfile message
├── Validate: User exists
├── Update: User profile với host information
│   ├── Languages spoken
│   ├── Host bio/description
│   ├── Contact preferences
│   └── Host status = Active
├── Store: Updated user entity
└── Publish: HostProfileUpdated event

Success Path:
HostProfileUpdated → Saga receives event → Transition to [Completed] → Finalize saga

Failure Path:
UpdateHostProfileFailed → Log error → Finalize saga (rollback)
```

**Business Rules & Validations**:

1. **Property Validation**:
   - PropertyType must exist và valid
   - Host must be authenticated user
   - Only one active property creation per host at a time
   - Location data must be complete (Address, City, Country, PostCode)

2. **RentalUnit Business Rules**:
   - Giá phải > 0 và currency valid
   - MaxAdults + MaxChildren > 0
   - Nếu RoomBased: Quantity phải > 0
   - Nếu EntireProperty: Size, BedroomsCount, BathroomsCount phải > 0

3. **Bedroom Configuration**:
   - Tổng số beds phải > 0
   - Bed types phải hợp lý với MaxAdults capacity
   - Bedroom count phải match với RentalUnit.BedroomsCount

4. **Host Profile Requirements**:
   - User must complete basic profile info
   - At least one language must be specified
   - Host agreement acceptance required

**Error Handling & Compensation**:

**Automatic Rollback Scenarios**:
- Service unavailable (timeout)
- Validation failures
- Database constraint violations
- Business rule violations

**Compensation Actions**:
```
Property Creation Failed:
→ No compensation needed (nothing created)

RentalUnit Creation Failed:
→ Delete created Property
→ Remove property images from storage

Bedroom Creation Failed:
→ Delete created RentalUnit
→ Delete created Property
→ Clean up related data

Host Profile Update Failed:
→ Mark Property as "Incomplete"
→ Send notification to user
→ Allow retry mechanism
```

**Draft Management Logic**:

**Draft Data Structure**:
```csharp
BecomeHostDraft {
    DraftId: Guid,
    HostId: int,
    CurrentStep: int,
    PropertyTypeId: int,
    PropertyName: string,
    LocationDto: { Address, City, Country, PostCode },
    RentalUnitDto: { Type, MaxAdults, MaxChildren, Price, Amenities },
    ListBedroomDtos: [{ SingleBeds, DoubleBeds, KingBeds, SofaBeds }],
    AmenityIds: List<int>,
    LanguageIds: List<int>,
    HouseRuleDto: { CheckIn/Out times, restrictions },
    Base64Images: List<string>,
    HostProfileDto: { Languages, Bio, ContactPreferences }
}
```

**Draft Lifecycle**:
1. **Creation**: User starts process → Draft saved to Redis với TTL 24h
2. **Updates**: Mỗi step completion → Update draft progress
3. **Validation**: Before each saga step → Validate draft data completeness
4. **Cleanup**: Saga completion → Remove draft from Redis
5. **Recovery**: Process failure → Restore từ draft để retry

**Retry & Recovery Mechanisms**:

**Saga Retry Logic**:
- Automatic retry cho transient failures (3 attempts)
- Exponential backoff cho message delivery
- Dead letter queue cho failed messages
- Manual retry via admin interface

**Data Consistency**:
- Saga correlation ID tracking qua tất cả services
- Idempotent message processing
- Database transactions cho atomic operations
- Event sourcing cho audit trail

**Monitoring & Observability**:

**Key Metrics**:
- Saga completion rate per step
- Average completion time
- Failure rates by step
- Draft abandonment rate

**Logging Points**:
- Saga state transitions
- Business rule validation results
- Error details với correlation ID
- Performance metrics per step

**User Experience Considerations**:

**Progress Tracking**:
- Real-time progress updates via SignalR
- Step completion confirmations
- Error notifications với recovery options
- Draft auto-save functionality

**User Communication**:
- Email notifications cho major milestones
- In-app notifications cho errors
- Progress dashboard trên host portal
- Help & support integration

### 3.4 Booking Service
**Chức năng**: Quản lý booking process (đang trong giai đoạn phát triển)

**Planned Features**:
- Create booking requests
- Validate availability
- Payment processing
- Booking confirmation

### 3.5 API Gateway Service
**Chức năng**: Entry point, routing và load balancing

**Features**:
- Request routing đến appropriate services
- Authentication validation
- Rate limiting
- Request/Response logging

### 3.6 Web UI (Next.js)
**Chức năng**: Frontend application cho end users

**Technologies**:
- Next.js 15.3.4
- React 19
- TailwindCSS
- TypeScript

## 4. Hướng dẫn Cài đặt, Build và Chạy Local

### 4.1 Prerequisites
- .NET 9.0 SDK
- Docker & Docker Compose
- Node.js 18+ (cho Web UI)
- PostgreSQL (nếu không dùng Docker)

### 4.2 Environment Setup

1. **Clone repository**:
```bash
git clone <repository-url>
cd Booking
```

2. **Environment Files**:
Tạo các file environment:
- `.env.local` (cho development)
- `.env.docker` (cho Docker)
- `.env.production` (cho production)

Example `.env.local`:
```
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=identitydb;Username=trung;Password=123
ConnectionStrings__Redis=localhost:6379
MessageBroker__Host=rabbitmq://localhost
MessageBroker__Username=guest
MessageBroker__Password=guest
```

### 4.3 Docker Development Setup

1. **Start dependencies**:
```bash
# Start databases, Redis, RabbitMQ, Seq
docker-compose up -d identity-db property-db orchestrator-db redis rabbitmq seq
```

2. **Verify services**:
- PostgreSQL: `localhost:5432, 5433, 5434`
- Redis: `localhost:6379`
- RabbitMQ Management: `http://localhost:15672` (guest/guest)
- Seq: `http://localhost:5341`

### 4.4 Build và Run Services

**Option 1: VS Code Tasks**
```bash
# Build Identity Service
Ctrl+Shift+P → "Run Task" → "build-identity"

# Build Gateway
Ctrl+Shift+P → "Run Task" → "build-gateway"
```

**Option 2: Command Line**
```bash
# Build all services
dotnet build Booking.sln

# Run Identity Service
cd services/IdentityService/Identity.Api
dotnet run

# Run Property Service  
cd services/PropertyService/Property.Api
dotnet run

# Run Orchestrator Service
cd services/OrchestratorService/Orchestrator.Api
dotnet run

# Run API Gateway
cd services/ApiGateway
dotnet run
```

### 4.5 Run Web UI
```bash
cd web-ui
npm install
npm run dev
```

### 4.6 Docker Compose Full Stack
```bash
# Run entire stack
docker-compose up -d

# View logs
docker-compose logs -f [service-name]

# Stop all
docker-compose down
```

### 4.7 Database Migrations
```bash
# Identity Service
cd services/IdentityService/Identity.Infrastructure
dotnet ef database update

# Property Service
cd services/PropertyService/Property.Infrastructure  
dotnet ef database update
```

## 5. Thư viện và Công nghệ sử dụng

### 5.1 Backend (.NET 9.0)

**Core Frameworks**:
- ASP.NET Core 9.0
- Entity Framework Core 9.0
- ASP.NET Core Identity

**Messaging & Communication**:
- MassTransit 8.5.1 (Message broker abstraction)
- RabbitMQ (Message broker implementation)

**Database**:
- PostgreSQL (Npgsql 9.0.3)
- Redis (StackExchange.Redis 2.8.58)

**Patterns & Architecture**:
- MediatR 13.0.0 (CQRS pattern)
- FluentValidation (Validation)
- Dapper (Database queries)

**Logging & Monitoring**:
- Serilog.AspNetCore 9.0.0
- Serilog.Sinks.Seq 9.0.0
- Serilog.Enrichers.CorrelationId 3.0.1

**Authentication**:
- System.IdentityModel.Tokens.Jwt 8.13.0

**Documentation**:
- Scalar.AspNetCore (API documentation)

### 5.2 Frontend
- Next.js 15.3.4
- React 19.0.0
- TailwindCSS 4
- TypeScript 5

### 5.3 Infrastructure
- Docker & Docker Compose
- PostgreSQL 16 Alpine
- Redis 7 Alpine
- RabbitMQ 3 Management
- Seq (Datalust)

### 5.4 Development Tools
- .NET CLI
- Entity Framework CLI tools
- ESLint (cho Frontend)

## 6. Các Thiết kế Đặc biệt

### 6.1 Saga Pattern (Orchestration-based)

**Implementation**: `BecomeHostSaga` trong Orchestrator Service

**Characteristics**:
- **Stateful**: Saga instance lưu state qua các bước
- **Orchestration**: Central coordinator điều khiển workflow
- **Compensation**: Automatic rollback khi có lỗi
- **Correlation**: Sử dụng CorrelationId để track workflow

**Example Flow**:
```csharp
Initially(
    When(Started)
        .Then(context => context.Saga.HostId = context.Message.HostId)
        .TransitionTo(CreatingProperty)
        .Publish(context => context.Saga.Draft.ToCreateProperty())
);

During(CreatingProperty,
    When(PropertyCreated)
        .Then(context => context.Saga.PropertyId = context.Message.PropertyId)
        .TransitionTo(AddingRentalUnit)
        .Publish(context => context.Saga.Draft.ToAddRentalUnit(context.Saga.PropertyId)),
    When(CreatePropertyFailed)
        .Then(ctx => _logger.LogError("Create Property Failed"))
        .Finalize()
);
```

### 6.2 Domain-driven Design (DDD)

**Aggregate Roots**:
- `Property` (Property Service)
- `User` (Identity Service)
- `BecomeHostSaga` (Orchestrator Service)

**Value Objects**:
- `Price` (Amount + Currency)
- `Location` (Address, City, Country, PostCode)
- `HouseRule` (CheckIn/Out times, restrictions)
- `RentalUnitType` (Room vs EntireProperty)

**Domain Events**:
- `PropertyCreated`
- `RentalUnitAdded`
- `BedroomAdded`
- `HostProfileUpdated`

### 6.3 CQRS Pattern

**Implementation**: MediatR cho Command/Query separation

**Commands**: Modify state (CreateProperty, AddRentalUnit)
**Queries**: Read data (GetProperty, GetUser)

**Benefits**:
- Separation of concerns
- Different optimization strategies
- Scalability cho read/write operations

### 6.4 Event-driven Architecture

**Message Types**:
- **Commands**: Request actions (`CreateProperty`)
- **Events**: Things that happened (`PropertyCreated`)
- **Faults**: Error handling (`Fault<CreateProperty>`)

**Communication Flow**:
```
Service A → Publish Command → Message Broker → Service B → Process → Publish Event → Service A
```

### 6.5 Repository Pattern + Unit of Work

**Generic Repository**:
```csharp
public interface IGenericRepository<T> where T : Entity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
```

### 6.6 Table Per Hierarchy (TPH)

**RentalUnit Inheritance**:
```csharp
RentalUnit (Abstract)
├── RoomRentalUnit (Name, Quantity, SharedBathroom)
└── EntirePropertyRentalUnit (Size, BedroomsCount, BathroomsCount)
```

**EF Configuration**:
```csharp
builder.HasDiscriminator<string>("RentalType")
    .HasValue<RoomRentalUnit>("RoomBased")
    .HasValue<EntirePropertyRentalUnit>("EntireProperty");
```

## 7. Hướng Mở rộng / Maintain về sau

### 7.1 Scaling Strategies

**Horizontal Scaling**:
- Deploy multiple instances của mỗi service
- Load balancer cho API Gateway
- Database read replicas
- Redis clustering

**Service Decomposition**:
- Tách Asset Service cho image/file management
- Payment Service cho payment processing
- Notification Service cho emails/SMS
- Analytics Service cho business intelligence

### 7.2 Performance Optimization

**Database**:
- Implement database indexing strategies
- Query optimization với Dapper
- Connection pooling
- Database sharding cho large datasets

**Caching**:
- Redis distributed caching
- Application-level caching
- CDN cho static assets
- Cache-aside pattern implementation

**Message Broker**:
- RabbitMQ clustering
- Message partitioning
- Dead letter queues
- Priority queues

### 7.3 Monitoring & Observability

**Implement**:
- Distributed tracing (OpenTelemetry)
- Application metrics (Prometheus + Grafana)
- Health checks cho các services
- Circuit breaker pattern
- Alerting system

**Logging Enhancements**:
- Structured logging với correlation IDs
- Log aggregation và analysis
- Performance metrics tracking
- Error rate monitoring

### 7.4 Security Enhancements

**Authentication & Authorization**:
- OAuth 2.0 / OpenID Connect
- Role-based access control (RBAC)
- API rate limiting
- Request validation middleware

**Data Protection**:
- Encrypt sensitive data at rest
- HTTPS enforced communication
- PCI compliance cho payment data
- GDPR compliance cho user data

### 7.5 DevOps & Deployment

**CI/CD Pipeline**:
- GitHub Actions / Azure DevOps
- Automated testing (Unit + Integration)
- Docker image building
- Blue-green deployment
- Database migration automation

**Infrastructure as Code**:
- Kubernetes deployment
- Helm charts
- Terraform cho cloud resources
- Environment management

### 7.6 Code Quality & Maintenance

**Code Standards**:
- SonarQube integration
- Code coverage requirements
- Automated code review
- Architecture decision records (ADRs)

**Testing Strategy**:
- Unit tests cho business logic
- Integration tests cho APIs
- Contract testing cho services
- Load testing cho performance
- E2E testing cho critical flows

### 7.7 Business Logic Extensions

**Booking Features**:
- Advanced search & filtering
- Real-time availability checking
- Dynamic pricing algorithms
- Review & rating system
- Multi-language support

**Host Features**:
- Advanced property management
- Revenue analytics
- Calendar integration
- Automated pricing suggestions
- Host communication tools

### 7.8 Technology Migration Paths

**Potential Upgrades**:
- .NET versions (automatic với LTS releases)
- Database upgrades (PostgreSQL versions)
- Message broker alternatives (Apache Kafka for high throughput)
- Frontend framework updates
- Cloud-native technologies (Service Mesh, Istio)

### 7.9 Data Strategy

**Analytics & BI**:
- Data warehouse setup
- ETL pipelines
- Business intelligence dashboard
- Machine learning integration
- Predictive analytics

**Data Governance**:
- Data retention policies
- Backup & disaster recovery
- Data quality monitoring
- Master data management

---

## Kết luận

Dự án Booking Platform được thiết kế với kiến trúc microservices hiện đại, áp dụng các pattern và best practices trong software engineering. Hệ thống có tính mở rộng cao, dễ bảo trì và có thể handle được business requirements phức tạp thông qua event-driven architecture và saga pattern.

Việc maintain và mở rộng hệ thống này cần focus vào monitoring, performance optimization, và gradual feature enhancement dựa trên user feedback và business needs.

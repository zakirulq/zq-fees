# ZQ.Fees - Student Fees Management API

A comprehensive .NET 9.0 Web API project implementing Clean Architecture with CQRS pattern for managing student fee payments. The system provides robust payment processing with validation, error handling, and comprehensive testing.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

- **Domain Layer**: Core business entities and enums
- **Application Layer**: Business logic, commands, handlers, and interfaces
- **Infrastructure Layer**: Data access and external dependencies
- **API Layer**: Controllers, middleware, and presentation logic

## 📁 Project Structure

```
ZQ.Fees/
├── src/
│   ├── ZQ.Fees.Api/                    # Web API Layer
│   │   ├── Controllers/                # API Controllers
│   │   │   └── FeesController.cs       # Payment endpoints
│   │   ├── Middleware/                 # Custom middleware
│   │   ├── Filters/                    # Action filters
│   │   ├── Models/                     # API request/response models
│   │   ├── Data/                       # DbContext configuration
│   │   ├── Properties/                 # Launch settings
│   │   ├── Program.cs                  # Application entry point
│   │   ├── appsettings.json           # Configuration
│   │   └── sample-requests.http        # HTTP request samples
│   │
│   ├── ZQ.Fees.Application/            # Application Layer
│   │   ├── Common/
│   │   │   └── Behaviors/              # MediatR behaviors
│   │   ├── Interfaces/                 # Application interfaces
│   │   ├── Payments/
│   │   │   └── Commands/
│   │   │       └── CreatePayment/      # Create payment command & handler
│   │   ├── Services/                   # Application services
│   │   └── DependencyInjection.cs     # Service registration
│   │
│   ├── ZQ.Fees.Domain/                 # Domain Layer
│   │   ├── Models/                     # Domain entities
│   │   │   ├── Payment.cs              # Payment entity
│   │   │   └── PaymentViewModel.cs     # Payment view model
│   │   ├── Enums/                      # Domain enums
│   │   │   ├── PaymentMethod.cs        # Payment method types
│   │   │   └── IdempotencyStatus.cs    # Idempotency statuses
│   │   └── ViewModels/                 # Domain view models
│   │
│   └── ZQ.Fees.Infrastructure/         # Infrastructure Layer
│       ├── Data/                       # Database context & configuration
│       ├── Repositories/               # Repository implementations
│       └── DependencyInjection.cs     # Infrastructure service registration
│
├── test/
│   ├── ZQ.Fees.Test.Unit/             # Unit Tests
│   │   ├── Controllers/               # Controller tests
│   │   ├── Handlers/                  # Command handler tests
│   │   └── Validators/                # Validation tests
│   │
│   └── ZQ.Fees.Test.Integration/      # Integration Tests
│       ├── Controllers/               # End-to-end controller tests
│       └── Infrastructure/            # Database integration tests
│
├── ZQ.Fees.sln                       # Solution file
└── README.md                         # Project documentation
```

## 🚀 Technology Stack

- **.NET 9.0** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core** - ORM with InMemory database
- **MediatR** - Mediator pattern implementation
- **FluentValidation** - Input validation library
- **Swagger/OpenAPI** - API documentation
- **NUnit** - Testing framework
- **MongoDB** - Database (orchestrated via Aspire in Docker)

## ✨ Features

- **Clean Architecture**: Well-organized, maintainable codebase
- **CQRS Pattern**: Commands and Queries separation
- **Validation Pipeline**: Comprehensive input validation with FluentValidation
- **Error Handling**: Global exception handling with custom filters
- **Idempotency**: Request idempotency support
- **API Documentation**: Interactive Swagger UI
- **Comprehensive Testing**: Unit and integration tests

## 📋 Prerequisites

Before running this application, ensure you have the following installed:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

## 🛠️ Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd ZQ.Fees
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Solution

```bash
dotnet build
```

### 4. Run Tests

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run specific test project
dotnet test test/ZQ.Fees.Test.Unit
dotnet test test/ZQ.Fees.Test.Integration
```

### 5. Start the Application

```bash
# Run the API
dotnet run --project src/ZQ.Fees.Api

# Or with watch mode for development
dotnet watch --project src/ZQ.Fees.Api
```

The API will be available at:
- HTTPS: `https://localhost:7000`
- HTTP: `http://localhost:5000`

## 📚 API Documentation

### Base URL
```
https://localhost:7000/api
```

### Endpoints

#### Create Payment
Creates a new payment record for a student.

**Endpoint:** `POST /api/fees/{studentId}/payments`

**Path Parameters:**
- `studentId` (integer, required): The unique identifier of the student

**Request Headers:**
```
Content-Type: application/json
```

**Request Body:**
```json
{
  "amount": 100.50,
  "method": 1
}
```

**Request Body Schema:**
- `amount` (decimal, required): Payment amount (must be greater than 0)
- `method` (integer, required): Payment method (1-5, see Payment Methods below)

**Success Response (200 OK):**
```json
{
  "data": {
    "paymentReferenceNumber": 12345,
    "studentId": 123,
    "amount": 100.50,
    "method": "CreditCard"
  },
  "message": "Payment created successfully"
}
```

**Validation Error Response (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Amount": ["Amount must be greater than 0"],
    "Method": ["Payment method must be a valid payment method"]
  }
}
```

### Payment Methods

| Value | Method      | Description                    |
|-------|-------------|--------------------------------|
| 1     | CreditCard  | Credit card payment           |
| 2     | DebitCard   | Debit card payment            |
| 3     | BankTransfer| Bank transfer payment         |
| 4     | Cash        | Cash payment                  |
| 5     | Check       | Check payment                 |

## 🧪 Sample Requests

### Valid Payment Request
```http
POST https://localhost:7000/api/fees/123/payments
Content-Type: application/json

{
  "amount": 100.50,
  "method": "CreditCard"
}
```

### Bank Transfer Payment
```http
POST https://localhost:7000/api/fees/456/payments
Content-Type: application/json

{
  "amount": 250.75,
  "method": "BankTransfer"
}
```

### Invalid Amount (Validation Error)
```http
POST https://localhost:7000/api/fees/789/payments
Content-Type: application/json

{
  "amount": 0,
  "method": "BankTransfer"
}
```

### Invalid Payment Method (Validation Error)
```http
POST https://localhost:7000/api/fees/789/payments
Content-Type: application/json

{
  "amount": 100.00,
  "method": "SomeInvalidMethod"
}
```

## ✅ Validation Rules

The API enforces the following validation rules:

### Path Parameters
- `studentId`: Must be greater than 0

### Request Body
- `amount`: 
  - Required field
  - Must be greater than 0
  - Supports decimal precision
- `method`:
  - Required field
  - Must be a valid string value
  - Corresponds to PaymentMethod enum

## 🧪 Testing

The project includes comprehensive test coverage:

### Unit Tests (`ZQ.Fees.Test.Unit`)
- **Command Validation Tests**: FluentValidation rule testing
- **Handler Tests**: Command handler logic testing
- **Domain Model Tests**: Entity and value object testing

### Integration Tests (`ZQ.Fees.Test.Integration`)
- **API Endpoint Tests**: Full HTTP request/response testing
- **Database Integration**: Entity Framework integration testing
- **Validation Pipeline Tests**: End-to-end validation testing

### Running Tests

```bash
# Run all tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run tests with detailed output
dotnet test --verbosity normal

# Run specific test category
dotnet test --filter "Category=Unit"
dotnet test --filter "Category=Integration"

# Run tests in watch mode
dotnet watch test
```

## 🐳 Docker & Development Environment

This project uses **Aspire** to orchestrate MongoDB in a Docker container for development.

### Starting the Development Environment

```bash
# Start MongoDB via Aspire (if configured)
# The application will automatically connect to the containerized database
dotnet run --project src/ZQ.Fees.Api
```

### Database Configuration

The application uses Entity Framework Core with:
- **Development**: InMemory database provider
- **Production**: MongoDB (via Aspire orchestration)

## 🔧 Configuration

### Application Settings

Key configuration options in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Environment Variables

- `ASPNETCORE_ENVIRONMENT`: Set to `Development` for development mode
- `ASPNETCORE_URLS`: Configure listening URLs

## 🚀 Deployment

### Building for Production

```bash
# Build in Release mode
dotnet build --configuration Release

# Publish the application
dotnet publish --configuration Release --output ./publish
```

### Running in Production

```bash
# Set production environment
export ASPNETCORE_ENVIRONMENT=Production

# Run the published application
dotnet ./publish/ZQ.Fees.Api.dll
```

## 🔍 Troubleshooting

### Common Issues

1. **Port Already in Use**
   ```bash
   # Find process using port 7000
   lsof -ti:7000
   # Kill the process
   kill -9 <PID>
   ```

2. **Database Connection Issues**
   - Ensure Docker Desktop is running
   - Verify Aspire orchestration is properly configured
   - Check Entity Framework migrations

3. **Build Errors**
   ```bash
   # Clean and rebuild
   dotnet clean
   dotnet restore
   dotnet build
   ```

## 📈 Performance Considerations

- **In-Memory Database**: Used for development; configure persistent storage for production
- **Validation Pipeline**: Efficient FluentValidation with MediatR behaviors
- **Async/Await**: All operations are asynchronous for better scalability
- **Dependency Injection**: Proper service lifetime management


## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

**Happy Coding! 🎉**

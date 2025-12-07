-----

# ByteStore - Modern Full-Stack E-Commerce Solution

**ByteStore** is a robust, modular, and scalable E-Commerce platform built to demonstrate advanced software engineering practices. It features a **Clean (Onion) Architecture** backend using ASP.NET Core 8 and a responsive, component-based frontend using Angular.

The system is designed with high performance, scalability, and maintainability in mind, utilizing modern DevOps practices and cloud-native patterns.

-----

## 🏗️ Architecture Overview

The project follows a **Monorepo** structure containing both the API and the Client application.

### Backend: Onion Architecture

The API allows for a strict separation of concerns, ensuring the Core domain remains independent of external frameworks.

  * **Domain (Core):** Entities, Enums, and Repository Interfaces. Pure C\#.
  * **Application (Core):** Business logic, Services, DTOs, Validators (FluentValidation), and AutoMapper profiles.
  * **Infrastructure (Persistence):** EF Core 8, PostgreSQL implementation, Redis caching, and Stripe integration.
  * **Presentation (API):** Controllers, Middleware, Authentication configuration, and Scalar API documentation.

### Frontend: Modular Angular

The client uses a feature-module strategy with **Lazy Loading** to optimize bundle sizes.

  * **Core Module:** Singleton services (Auth, HTTP Interceptors).
  * **Shared Module:** Reusable UI components and Pipes.
  * **Feature Modules:** Auth, Cart, Catalog, Checkout, Admin (Protected by Guards).

-----

## 🚀 Tech Stack

| Area | Technology |
| :--- | :--- |
| **Backend Framework** | ASP.NET Core 8 Web API |
| **Database** | PostgreSQL (Production), EF Core 8 (ORM) |
| **Caching** | Redis (Distributed Caching for Cart & Catalog) |
| **Frontend** | Angular, SCSS, RxJS, bootstrap and ng-bootstrap |
| **Authentication** | ASP.NET Core Identity + JWT (Access/Refresh Tokens) |
| **Validation** | FluentValidation |
| **Logging & Monitoring** | Serilog, Health Checks |
| **Payment Gateway** | Strip |
| **File Storage** | ImageKit.io |
| **Email Service** | Brevo (formerly Sendinblue) |
| **Containerization** | Docker & Docker Compose |

-----

## 📂 Repository Structure

```plaintext
/ByteStore
  ├── /src
  │   ├── /Core
  │   │     ├── /Domain          # Enterprise Logic & Entities
  │   │     └── /Application     # Use Cases, DTOs, Interfaces
  │   ├── /External
  │   │     ├── /Api             # Entry Point, Middleware, DI
  │   │     ├── /Persistence     # DB Context, Migrations, Repositories
  │   │     └── /Presentation    # Controllers, Endpoints
  ├── /Client                    # Angular SPA
  ├── /test                      # xUnit Tests
  ├── docker-compose.yml
  └── .env
```

-----

## ✨ Key Features

### 🛒 Shopping Experience

  * **Catalog Management:** Advanced filtering, sorting, and pagination for products.
  * **Smart Cart:** Persistent shopping cart powered by **Redis** (merges anonymous cart upon login).
  * **Checkout Flow:** Secure payment processing using **Stripe Elements**.
  * **Real-time Stock:** Validation preventing purchase of out-of-stock items.

### 🔐 Security & Identity

  * **Role-Based Access Control (RBAC):** Distinct flows for `Customers` and `Admins`.
  * **Token Management:** Secure JWT implementation with Refresh Token rotation and IP rate limiting.
  * **Email Verification:** Account confirmation flows via Brevo.

### ⚡ Performance & Reliability

  * **Caching Strategy:** Output caching for catalogs and distributed caching for sessions.
  * **Resilience:** Global Exception Handling (RFC 7807 ProblemDetails) and Retries.
  * **Health Checks:** Dedicated endpoints for liveness and readiness probes.

-----

## 🛠️ Getting Started

### Prerequisites

  * [.NET 8 SDK](https://dotnet.microsoft.com/download)
  * [Node.js (LTS)](https://nodejs.org/)
  * [Docker Desktop](https://www.docker.com/products/docker-desktop)

### **Environment & Configuration**

### Required Settings

```
ConnectionStrings:Default
Jwt:Issuer
Jwt:Audience
Jwt:Key
Redis:Connection
Stripe:ApiKey
Stripe:WebhookSecret
Serilog
```

### 🐳 Running with Docker (Recommended)

To spin up the Database, Redis, Backend, and Frontend simultaneously:

```bash
# From the root directory
docker-compose up --build
```

### 💻 Running Locally (Manual)

**1. Backend**

```bash
cd src/External/Api
# Run Migrations (Specify Context due to HealthChecks)
dotnet ef database update --context AppDbContext --project ../Persistence
# Start API
dotnet run
```

*API will be available at `https://localhost:7001`*

**2. Frontend**

```bash
cd Client
npm install
ng serve
```

*Client will be available at `http://localhost:4200`*

-----

## 🧪 Testing

The project maintains high code quality via unit and integration tests.

```bash
# Run all tests
dotnet test

# Note: Tests use TestAsyncEnumerable to mock EF Core async operations.
```

-----

## ⚠️ Troubleshooting & Notes

  * **Migration Error:** If you encounter a "More than one DbContext" error during migrations (due to HealthChecks), always specify the context:
    `dotnet ef migrations add <Name> --context AppDbContext`
  * **Payments:** Webhooks are required for order status updates. Ensure Stripe CLI is forwarding events to your local endpoint during development.
  * **Images:** Image upload requires valid ImageKit.io credentials configured in the DI container.

-----

## 📜 License

Distributed under the MIT License. See `LICENSE` for more information.

-----

**Developed by Ibrahim Hany**

-----

✅ **Swagger documentation**
✅ **System architecture diagram (ASCII or image)**
✅ **Frontend README**
✅ **docker-compose.yml optimized for production**

Just tell me!

# Vehicle Listing API

[![Build and Test](https://github.com/samballington/listing_api/actions/workflows/ci.yml/badge.svg)](https://github.com/samballington/listing_api/actions/workflows/ci.yml)
[![Tests](https://img.shields.io/badge/tests-9%20passing-brightgreen)](https://github.com/samballington/listing_api/actions/workflows/ci.yml)
[![.NET](https://img.shields.io/badge/.NET-10.0-512bd4)](https://dotnet.microsoft.com)
[![Docker](https://img.shields.io/badge/docker-ready-2496ed?logo=docker&logoColor=white)](https://www.docker.com)

A RESTful vehicle inventory management API for car dealerships, built as a portfolio project demonstrating clean layered architecture, interface-driven design, and production-quality engineering practices.
[**Check out the live demo**](http://3.235.181.210:8080)
## Tech Stack

- **.NET 10** — backend runtime
- **ASP.NET Core** — web framework
- **Entity Framework Core 10** — ORM with SQL Server provider
- **SQL Server 2022** — relational database (Docker image)
- **FluentValidation** — input validation
- **Serilog** — structured JSON logging (console + file sinks)
- **XUnit + Moq** — unit testing
- **Docker + docker-compose** — containerization
- **GitHub Actions** — CI pipeline
- **AWS EC2** - Hosting

## Architecture

```
Controller → Service → Repository → DbContext → SQL Server
```

Each layer communicates through interfaces. Controllers handle HTTP only. Services contain all business logic. Repositories handle EF Core queries. DTOs are used at every API boundary — entity models are never exposed.

## How to Run (Docker)

Requires [Docker Desktop](https://www.docker.com/products/docker-desktop/).

```bash
git clone https://github.com/samballington/listing_api.git
cd listing_api/VehicleListing
docker-compose up --build
```

The API will be available at `http://localhost:8080`.

SQL Server starts first with a health check, then the API connects and applies EF migrations automatically.

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/vehicles` | List vehicles (supports filtering) |
| GET | `/api/vehicles/{id}` | Get vehicle by ID |
| POST | `/api/vehicles` | Create a vehicle |
| PUT | `/api/vehicles/{id}` | Update a vehicle |
| DELETE | `/api/vehicles/{id}` | Delete a vehicle |
| GET | `/api/vehicles/search?query=` | Search vehicles by make/model/description |
| GET | `/api/dealers` | List all dealers |
| GET | `/api/dealers/{id}` | Get dealer by ID (includes their vehicles) |

### Filter Parameters for `GET /api/vehicles`

| Parameter | Type | Example |
|-----------|------|---------|
| `make` | string | `?make=Toyota` |
| `model` | string | `?model=Camry` |
| `year` | int | `?year=2022` |
| `minPrice` | decimal | `?minPrice=20000` |
| `maxPrice` | decimal | `?maxPrice=50000` |
| `status` | string | `?status=Available` |

### Vehicle Status Values

`Available` · `Sold` · `Pending`

## How to Run Tests

```bash
cd listing_api
dotnet test VehicleListing.slnx
```

Expected output: 9 tests, all passing. Tests use Moq to mock repositories — no database connection required.

## Project Structure

```
VehicleListing/
├── src/
│   └── VehicleListing.Api/
│       ├── Controllers/
│       ├── Services/
│       ├── Repositories/
│       ├── Data/
│       │   └── Migrations/
│       ├── Models/
│       ├── DTOs/
│       ├── Validators/
│       ├── Program.cs
│       └── appsettings.json
├── tests/
│   └── VehicleListing.Tests/
├── Dockerfile
├── docker-compose.yml
└── README.md
```

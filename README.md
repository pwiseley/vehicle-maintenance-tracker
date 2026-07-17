# 🔧 Vehicle Maintenance Tracker

> Fleet maintenance tracking API : logs service history per vehicle, tracks maintenance status, and surfaces fleet-wide metrics through a dashboard endpoint.

![Status](https://img.shields.io/badge/status-in%20development-yellow)
![C#](https://img.shields.io/badge/C%23-12-purple)
![.NET](https://img.shields.io/badge/ASP.NET%20Core-8.0-blueviolet)
![React](https://img.shields.io/badge/React-TypeScript-blue)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-blue)

---

## What it does

Vehicle Maintenance Tracker lets fleet operators register vehicles, log maintenance interventions against them, and follow each intervention through a status workflow (scheduled → in progress → completed). A dashboard endpoint aggregates fleet-wide metrics: total maintenance cost, upcoming interventions, and status distribution.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | C# 12, ASP.NET Core 8, NuGet |
| Frontend | React, TypeScript, Vite |
| Database | PostgreSQL, Entity Framework Core |
| Architecture | Controller / Service / DbContext |

---

## Key Features

- Vehicle registry with type classification (car, truck, snow plow, service vehicle)
- Maintenance history per vehicle with cost and mileage tracking
- Status workflow for each maintenance record
- Dashboard endpoint with LINQ-based aggregations
- RESTful API documented with Swagger / OpenAPI
- EF Core migrations for versioned schema changes

---

## API Endpoints

| Method | Route | Purpose |
|---|---|---|
| `POST` | `/api/vehicles` | Register a vehicle |
| `GET` | `/api/vehicles` | List all vehicles |
| `GET` | `/api/vehicles/{id}` | Get vehicle details |
| `POST` | `/api/vehicles/{id}/maintenance` | Log a maintenance record |
| `GET` | `/api/vehicles/{id}/maintenance` | List a vehicle's maintenance history |
| `PATCH` | `/api/maintenance/{id}/status` | Advance maintenance status |
| `GET` | `/api/dashboard` | Fleet-wide metrics |

---

## Example HTTP Response (Backend)

```json
{
  "totalVehicles": 12,
  "totalMaintenanceCost": 4580.50,
  "upcomingMaintenanceCount": 3,
  "maintenanceByStatus": {
    "Scheduled": 3,
    "InProgress": 1,
    "Completed": 8
  }
}
```

---

## Live Demo

Not deployed — runs locally with `dotnet run` and `npm run dev`.

---

## Related

- [insurance-quote](https://github.com/pwiseley/insurance-quote) : Spring Boot + React, live at `primio.petiton.dev`
- [url-shortener](https://github.com/pwiseley/url-shortener) : Spring Boot + Angular, live at `go.petiton.dev`
- [Floppa Marketplace](https://github.com/pwiseley/Floppa-app) : Java REST API, three-layer architecture, CI/CD
- [Portfolio](https://petiton.dev)

# Vehicle Maintenance Tracker

[Lire en français](README.FR.md)

Fleet maintenance tracking API: logs service history per vehicle, tracks maintenance status, and surfaces fleet-wide metrics through a dashboard endpoint.

> [Live Demo](https://vmt.petiton.dev)

## Contents
- [Tech Stack](#tech-stack)
- [API](#api)
- [Deployment](#deployment)
- [Related](#related)

## Tech Stack

| Layer | Technology |
|---|---|
| **Backend** | C#, ASP.NET Core 10, Entity Framework Core |
| **Frontend** | React, TypeScript, Vite |
| **Database** | PostgreSQL 16 |
| **Testing** | xUnit, Testcontainers, Respawn, FluentAssertions |
| **CI/CD** | Azure DevOps Pipelines |
| **Hosting** | Azure Container Apps, Azure Container Registry, nginx |
| **Architecture** | Controller / Service / DbContext |

## API

> [API Docs (Swagger)](https://vmt.petiton.dev/swagger)

| Method | Route | Purpose |
|---|---|---|
| `POST` | `/api/vehicles` | Register a vehicle |
| `GET` | `/api/vehicles` | List all vehicles |
| `GET` | `/api/vehicles/{id}` | Get vehicle details |
| `POST` | `/api/vehicles/{id}/maintenance` | Log a maintenance record |
| `GET` | `/api/vehicles/{id}/maintenance` | List a vehicle's maintenance history |
| `PATCH` | `/api/maintenance/{id}/status` | Advance maintenance status |
| `GET` | `/api/dashboard` | Fleet-wide metrics |

Registering a vehicle with a plate number that already exists returns `409 Conflict` in **ProblemDetails** format:

```json
{
  "title": "Resource already exists",
  "status": 409,
  "detail": "A vehicle with plate number 'ABC123' already exists."
}
```

## Deployment

Images are built by the **Azure DevOps** pipeline, stored in **Azure Container Registry**, and deployed to **Azure Container Apps**.

![Azure Container Apps overview](docs/images/azure-container-apps.png)

![Azure DevOps pipeline run](docs/images/azure-pipeline.png)

![Azure Container Registry](docs/images/azure-registry.png)

## Related

- [insurance-quote](https://github.com/pwiseley/insurance-quote): Spring Boot + React, live at `primio.petiton.dev`
- [url-shortener](https://github.com/pwiseley/url-shortener): Spring Boot + Angular, live at `go.petiton.dev`
- [Floppa Marketplace](https://github.com/pwiseley/Floppa-app): Java REST API, three-layer architecture, CI/CD
- [Portfolio](https://petiton.dev)

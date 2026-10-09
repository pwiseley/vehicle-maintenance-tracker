# Vehicle Maintenance Tracker

[Read in English](README.md)

API de suivi de l'entretien d'une flotte de véhicules : historique des maintenances par véhicule, suivi de leur statut et indicateurs globaux de la flotte via un endpoint de tableau de bord.

> [Démo en ligne](https://vmt.petiton.dev)

## Sommaire
- [Technologies](#technologies)
- [API](#api)
- [Déploiement](#déploiement)
- [Projets connexes](#projets-connexes)

## Technologies

| Couche | Technologie |
|---|---|
| **Backend** | C#, ASP.NET Core 10, Entity Framework Core |
| **Frontend** | React, TypeScript, Vite |
| **Base de données** | PostgreSQL 16 |
| **Tests** | xUnit, Testcontainers, Respawn, FluentAssertions |
| **CI/CD** | Azure DevOps Pipelines |
| **Hébergement** | Azure Container Apps, Azure Container Registry, nginx |
| **Architecture** | Controller / Service / DbContext |

## API

> [Documentation de l'API (Swagger)](https://vmt.petiton.dev/swagger)

| Méthode | Route | Rôle |
|---|---|---|
| `POST` | `/api/vehicles` | Enregistrer un véhicule |
| `GET` | `/api/vehicles` | Lister les véhicules |
| `GET` | `/api/vehicles/{id}` | Détails d'un véhicule |
| `POST` | `/api/vehicles/{id}/maintenance` | Ajouter une maintenance |
| `GET` | `/api/vehicles/{id}/maintenance` | Historique d'entretien d'un véhicule |
| `PATCH` | `/api/maintenance/{id}/status` | Faire avancer le statut d'une intervention |
| `GET` | `/api/dashboard` | Indicateurs de la flotte |

Enregistrer un véhicule avec une plaque déjà existante retourne `409 Conflict` au format **ProblemDetails** :

```json
{
  "title": "Resource already exists",
  "status": 409,
  "detail": "A vehicle with plate number 'ABC123' already exists."
}
```

## Déploiement

Les images sont construites par le pipeline **Azure DevOps**, stockées dans **Azure Container Registry** et déployées sur **Azure Container Apps**.

![Vue d'ensemble Azure Container Apps](docs/images/azure-container-apps.png)

![Exécution du pipeline Azure DevOps](docs/images/azure-pipeline.png)

![Azure Container Registry](docs/images/azure-registry.png)

## Projets connexes

- [insurance-quote](https://github.com/pwiseley/insurance-quote) : Spring Boot + React, en ligne sur `primio.petiton.dev`
- [url-shortener](https://github.com/pwiseley/url-shortener) : Spring Boot + Angular, en ligne sur `go.petiton.dev`
- [Floppa Marketplace](https://github.com/pwiseley/Floppa-app) : API REST Java, architecture en trois couches, CI/CD
- [Portfolio](https://petiton.dev)

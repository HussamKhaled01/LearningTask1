# Business Card Management API

RESTful API built with **.NET 10** and **SQL Server** for managing business cards.

---

## Features

- CRUD operations for business cards
- SQL Server (Code First) with seeded data
- Basic validation (required fields, valid email)
- Proper HTTP status codes
- Layered architecture (Controllers, Services, Data, Models)
- OpenAPI documentation via Scalar UI

---

## Tech Stack

- **Backend:** .NET 10, ASP.NET Core Web API  
- **Database:** SQL Server, Entity Framework Core  
- **API Docs:** OpenAPI + Scalar  
- **Version Control:** Git & GitHub 


---

## Database Setup

1. Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=BusinessCardDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}

update the database:

This will create the database and insert seeded data.

Running the Application

Open Scalar UI to test endpoints:

https://localhost:{port}/scalar
API Endpoints
Method	Endpoint	Description
GET	/api/businesscards	Get all business cards
GET	/api/businesscards/{id}	Get a business card by ID
POST	/api/businesscards	Create a new business card
PUT	/api/businesscards/{id}	Update a business card
DELETE	/api/businesscards/{id}	Delete a business card
HTTP Status Codes

200 OK – Successful request

201 Created – Resource created

400 Bad Request – Validation failed

404 Not Found – Resource not found

500 Internal Server Error – Server error

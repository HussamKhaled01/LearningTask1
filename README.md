
# Business Card Management System

  

A full-stack web application designed for seamlessly managing business contacts. Built with a robust **.NET 10 Web API** backend and a sleek, modern **Angular 21** frontend.

  

---

  

## Key Features

  

-  **Full CRUD Operations**: Create, Read, Update, and Delete business cards.

-  **Image Uploads**: Native support for attaching profile pictures/logos securely.

-  **Advanced Data Grid**:

- Server-side Pagination

- Real-time Global Search Filtering

- Date of Birth & Gender Filtering

-  **Export & Import**:

- Export your filtered grid data to **CSV** or **XML**.

- Bulk import new business cards from existing `.csv` or `.xml` lists.

-  **QR Code Scanning**:

- Integrated camera support via `zxing`.

- Instantly scan a vCard QR code to auto-populate the "Add New Business Card" form!

-  **Global Error Handling**:

- Clean `ExceptionMiddleware` intercepts crashes, guaranteeing unified `500 Internal Server Error` JSON responses.

- Zero raw stack traces leaked to the frontend.

-  **Unit Testing**:

-  `xUnit` test suite with `Moq`.

- Uses `EntityFrameworkCore.InMemory` to simulate rapid database workflows without touching SQL Server.

  

---

  

## Tech Stack

  

-  **Frontend:** Angular 21 

-  **Backend:** .NET 10, ASP.NET Core Web API, C#

-  **Database:** SQL Server, Entity Framework Core (Code First)

-  **API Documentation:** OpenAPI + Scalar UI

-  **Testing:** xUnit, Moq

  

---

  

## Getting Started

  

### 1. Database Setup

Ensure you have SQL Server running locally. Update the database connection string in `backend/LearningTask1/appsettings.json`:

  

```json

{

"ConnectionStrings": {

"DefaultConnection": "Server=.;Database=BusinessCardDb;Trusted_Connection=True;TrustServerCertificate=True;"

}

}

```

  

Then, apply the Entity Framework migrations from the `backend/LearningTask1` directory:

```bash

dotnet  ef  database  update

```

*(This will automatically create the database and insert seeded dummy data!)*

  

### 2. Running the Backend

From the `backend/LearningTask1` directory:

```bash

dotnet  run

```

You can view the interactive API documentation at: `https://localhost:7193/scalar`

  

### 3. Running the Frontend

From the `frontend/LearningTask1-UI` directory, install dependencies and start the Angular server:

```bash

npm  install  --legacy-peer-deps

npm  start

```

Navigate to `http://localhost:4200` to view the application!

  

---

  

## Running Tests

To run the automated backend unit test suite, navigate to `backend/LearningTask1.Tests` and execute:

```bash

dotnet  test

```

  

To run the frontend component testing suite:

```bash

npm  test

```

  

---

  

## API Endpoints

| Method | Endpoint | Description |


| `GET` | `/api/businesscard` | Get paginated & filtered business cards |

| `GET` | `/api/businesscard/{id}` | Get a specific business card |

| `POST` | `/api/businesscard` | Form-data: Create a new card (supports image upload) |

| `PUT` | `/api/businesscard/{id}` | Form-data: Update an existing card |

| `DELETE`| `/api/businesscard/{id}` | Delete a card |

| `GET` | `/api/businesscard/export` | Export filtered data (Query: `?format=xml` or `csv`) |

| `POST` | `/api/businesscard/import` | Bulk import cards from `.xml` or `.csv` files |
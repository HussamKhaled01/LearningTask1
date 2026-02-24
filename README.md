📇 Business Card Management API
📌 Overview

This project is a backend training assignment focused on building a clean and structured RESTful API using modern .NET technologies.

The system manages business cards using .NET 10 Web API and SQL Server, following a layered architecture with validation, proper HTTP status codes, seeded data, and OpenAPI documentation rendered using Scalar UI.

🎯 Objectives Covered

Build RESTful APIs using ASP.NET Core

Integrate SQL Server with Entity Framework Core

Apply validation and structured error handling

Follow layered architecture principles

Return proper HTTP status codes

Document APIs using OpenAPI

✅ Implemented Features
🧾 Business Card Entity

The application manages business cards with the following fields:

Name

Gender

Date of Birth

Email

Phone

Address

🔄 CRUD Operations

Create business card

Get all business cards

Get business card by ID

Update business card

Delete business card

All endpoints return appropriate HTTP status codes.

🗄️ Database

SQL Server

Entity Framework Core (Code First)

Migrations enabled

Seeded initial data

Running database update will:

Create the database

Apply migrations

Insert seeded records automatically

✔️ Validation

Validation is implemented using Data Annotations:

Required fields

Valid email format

Invalid requests return 400 BadRequest.

📚 API Documentation (OpenAPI + Scalar)

The project uses OpenAPI to generate API documentation and Scalar as the interactive UI.

After running the application, Scalar UI is available at:

https://localhost:{port}/scalar

Scalar allows:

Viewing all endpoints

Testing API requests

Inspecting request and response schemas

🏗️ Project Structure
BusinessCardProject
│
├── Controllers
├── Services
├── Data
├── Models
Layer Responsibilities

Controllers → Handle HTTP requests and responses

Services → Contain business logic

Data → DbContext and database configuration

Models → Entity definitions

This layered structure improves maintainability and separation of concerns.

🛠️ Technologies Used

.NET 10

ASP.NET Core Web API

SQL Server

Entity Framework Core

OpenAPI

Scalar UI

Git & GitHub

▶️ How to Run the Project
1️⃣ Clone the Repository
git clone https://github.com/YOUR_USERNAME/YOUR_REPOSITORY.git
2️⃣ Configure Database

Update appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=BusinessCardDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

Make sure SQL Server is running.

3️⃣ Apply Migrations & Create Database
dotnet ef database update

This will:

Create the database

Apply migrations

Insert seeded data

4️⃣ Run the Application
dotnet run

Then open:

https://localhost:{port}/scalar
📡 API Endpoints
Method	Endpoint	Description
GET	/api/businesscards	Get all cards
GET	/api/businesscards/{id}	Get card by ID
POST	/api/businesscards	Create card
PUT	/api/businesscards/{id}	Update card
DELETE	/api/businesscards/{id}	Delete card
🔒 HTTP Status Codes Used

200 OK

201 Created

400 Bad Request

404 Not Found

500 Internal Server Error

📈 Future Enhancements

Angular frontend

Pagination

Filtering

CSV import/export

Photo upload

Unit testing

👨‍💻 Author

Hussam Khaled

If you want, I can refine this further

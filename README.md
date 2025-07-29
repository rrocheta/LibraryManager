## 📚 LibraryManager

A full-stack application for managing a library system, built with ASP.NET Core (Web API), React, and PostgreSQL. It allows you to create, edit, borrow, return, and manage books.

---

## 🚀 Features

- 📖 Create, edit, and delete books
- 📚 List available and borrowed books
- 🔄 Borrow and return books
- ✅ Backend unit tests using xUnit
- 🐳 Dockerized frontend, backend, and PostgreSQL database setup with migrations applied automatically

---

## 🧰 Tech Stack

| Layer     | Technology                    |
|-----------|-------------------------------|
| Frontend  | React                         |
| Backend   | ASP.NET Core Web API (.NET 8) |
| Database  | PostgreSQL                    |
| Styling   | None (barebones UI)           |
| Testing   | xUnit                         |
| DevOps    | Docker & Docker Compose       |

---

## 📂 Project Structure

```
LibraryManager/
├── backend/
│   └── LibraryManager.API/        # ASP.NET Core Web API
├── frontend/
│   └── React App                  # React SPA
├── docker-compose.yml
└── README.md
```

---

## ⚙️ Prerequisites

- Docker  
- Docker Compose  

> Migrations are automatically applied when the backend container starts.

(Optional, for local development and managing migrations):  
- .NET SDK

---

## 🐳 Running the Project (Docker Compose)

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/LibraryManager.git
   cd LibraryManager
   ```

2. **Build and start the containers**
   ```bash
   docker-compose up --build
   ```

3. The backend container will automatically apply Entity Framework Core migrations on startup, creating the necessary database schema.

4. **Access the application**
   - Backend API: [http://localhost:8080](http://localhost:8080)
   - Frontend: [http://localhost:3000](http://localhost:3000)

---

## 🔧 Running Backend Migrations Locally

If you want to create or update migrations locally, ensure you have the EF Core CLI tools installed:

```bash
dotnet tool install --global dotnet-ef
```

To add a new migration:

```bash
dotnet ef migrations add YourMigrationName
```

To update the database locally:

```bash
dotnet ef database update
```

---

## 🔬 Running Backend Tests (xUnit)

To run backend tests locally:

1. Navigate to the test project:
   ```bash
   cd backend/LibraryManager.Tests
   ```

2. Run tests with the .NET CLI:
   ```bash
   dotnet test
   ```

---

## 🧪 API Endpoints (Sample)

| Method | Endpoint             | Description           |
|--------|----------------------|-----------------------|
| GET    | `/api/books`         | List all books        |
| GET    | `/api/authors`       | List all authors      |
| GET    | `/api/publishers`    | List all publishers   |
| POST   | `/api/books`         | Create new book       |
| PUT    | `/api/books/{id}`    | Edit book             |
| DELETE | `/api/books/{id}`    | Delete book           |

---

## 📦 To Do / Improvements

- 🎨 Add UI styling (currently no design system or template used)
- 🧪 Add more test coverage (integration tests, frontend tests)

---

## 👨‍💻 Author

**Ricardo Rocheta**

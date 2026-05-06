# Car Management System

## Application Description
This project is a full-stack Car Management System with:
- A React frontend for authentication and CRUD operations.
- An ASP.NET Core Web API backend.
- SQL Server as the database.

The system allows users to manage:
- Cars
- Manufacturers
- Engines
- Car profiles (including photo upload)
- Car-engine relationships

Authentication uses JWT stored in HTTP-only cookies. The UI supports role-based access for User, Instructor/Employee, and Admin roles.

## Setup Instructions

### Prerequisites
- Node.js 18+
- npm
- .NET 8 SDK
- Docker Desktop (recommended for SQL Server)

### 1 Backend Setup (ASP.NET Core API)

1. Open a terminal and go to backend folder:
   - cd ..\WebApplication3

2. Start SQL Server with Docker:
   - docker compose up -d

3. Make sure database credentials match in both files:
   - WebApplication3\docker-compose.yml
   - WebApplication3\appsettings.json

4. Restore .NET packages:
   - dotnet restore

5. Apply migrations:
   - dotnet ef database update

6. Run backend:
   - dotnet run

Backend base URL:
- http://localhost:5000

Swagger:
- http://localhost:5000/swagger

### 2 Frontend Setup (React + Vite)

1. Open a terminal and go to frontend folder:
   - cd react-frontend

2. Install dependencies:
   - npm install

3. Create .env file in react-frontend (optional):
   - VITE_API_URL=http://localhost:5000/api

4. Start frontend:
   - npm run dev

Frontend URL:
- http://localhost:5173

## API Routes Used
Base prefix: /api

### Authentication
- POST /auth/login
- POST /auth/signup
- POST /auth/refresh-token

### Cars
- GET /car
- GET /car/{id}
- POST /car
- PUT /car/{id}
- DELETE /car/{id}

### Manufacturers
- GET /manufacturer
- GET /manufacturer/{id}
- POST /manufacturer
- PUT /manufacturer/{id}
- DELETE /manufacturer/{id}

### Engines
- GET /engine
- GET /engine/{id}
- POST /engine
- PUT /engine/{id}
- DELETE /engine/{id}

### Car Profiles
- GET /carprofiles
- GET /carprofiles/{id}
- POST /carprofiles
- POST /carprofiles/upload-photo
- PUT /carprofiles/{id}
- DELETE /carprofiles/{id}

### Car Engines
- GET /carengines
- GET /carengines/{carId}/{engineId}
- POST /carengines
- PUT /carengines/{carId}/{engineId}
- DELETE /carengines/{carId}/{engineId}

## Screenshots
### Login Screen

### Signup Screen

### Home Screen

### Cars List Screen

### Car Details Screen

### Create Car Screen

### Edit Car Screen

### Manufacturers List Screen

### Create Manufacturer Screen

### Engines List Screen

### Car Profiles List Screen

### Car Engines List Screen

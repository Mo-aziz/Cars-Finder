# WebApplication3 - Car Management API

A comprehensive ASP.NET Core Web API for managing cars, engines, manufacturers, and car profiles with cookie-based authentication, role-based authorization, and complete CRUD operations.

## Project Overview

This is an enterprise-grade ASP.NET Core 8 Web API that demonstrates best practices for building secure, scalable web services. The project implements all required learning objectives including entity relationships, dependency injection, clean service layers, DTOs, validation, cookie-based authentication, role-based authorization, and query optimization.

## Key Features

### Entity Management
- **Car Management**: Full CRUD operations with validation (Brand, Model, Year, Manufacturer)
- **Engine Management**: Manage different engine types and horsepower ratings
- **Manufacturer Management**: Track car manufacturers and their origins
- **Car Profiles**: Store additional car details like color, price, and description
- **Car-Engine Relationships**: Many-to-many relationship with installation dates

### Advanced Features
- **Hybrid JWT-in-Cookie Authentication**: JWT tokens stored in HTTP-only cookies for secure, stateless authentication
- **Role-Based Authorization**: Admin, Instructor, and User roles
- **Entity Framework Core**: With async database operations and automatic migrations
- **DTO Pattern**: Separate data transfer objects for API requests/responses
- **Request Validation**: Data annotation attributes (Required, StringLength, Range, etc.)
- **Query Optimization**: AsNoTracking() for read-only queries, LINQ Select projections
- **Swagger/OpenAPI**: Interactive API documentation
- **Docker Support**: Includes docker-compose configuration for SQL Server

## Technologies Used

- **.NET 8.0** - Framework
- **ASP.NET Core MVC** - Web framework
- **Razor Views** - Templating engine
- **Bootstrap 5** - CSS framework
- **C# 12** - Programming language
- **Entity Framework Core** - ORM for database access
- **SQL Server 2019** - Database (via Docker)
- **Docker** - Containerization for database


### Core Framework
- **ASP.NET Core 8**: Modern, lightweight, cross-platform web framework for building web APIs and services
- **Entity Framework Core 8**: Object-relational mapping (ORM) framework for database access with LINQ support
- **SQL Server**: Relational database management system (via Docker)

### Authentication & Security
- **JWT Bearer Authentication**: Industry-standard JWT tokens for stateless authentication
- **HTTP-Only Cookies**: JWT tokens stored in HTTP-only cookies to prevent XSS attacks
- **Microsoft.AspNetCore.Authentication.JwtBearer**: Validates JWT tokens extracted from cookies in API requests
- **Claims-Based Authorization**: User roles stored as claims within JWT tokens

### API Documentation
- **Swagger/OpenAPI**: Tools for documenting and testing RESTful APIs with interactive UI

### Data Validation
- **System.ComponentModel.DataAnnotations**: Built-in attributes for validating DTOs including Required, StringLength, Range, EmailAddress

## API Endpoints

### Authentication
- `POST /api/auth/login` - Login and set authentication cookie (username: admin, password: password for Admin role)
- `POST /api/auth/logout` - Logout and clear authentication cookie
- `POST /api/auth/register` - Register new user
- `GET /api/auth/profile` - Get authenticated user profile [Authorize]

### Cars
- `GET /api/car` - List all cars [Authorize: User, Instructor, Admin]
- `GET /api/car/{id}` - Get car details [Authorize: User, Instructor, Admin]
- `POST /api/car` - Create car [Authorize: Instructor, Admin]
- `PUT /api/car/{id}` - Update car [Authorize: Admin]
- `DELETE /api/car/{id}` - Delete car [Authorize: Admin]

### Engines
- `GET /api/engine` - List all engines [Authorize: User, Instructor, Admin]
- `GET /api/engine/{id}` - Get engine details [Authorize: User, Instructor, Admin]
- `POST /api/engine` - Create engine [Authorize: Instructor, Admin]
- `PUT /api/engine/{id}` - Update engine [Authorize: Admin]
- `DELETE /api/engine/{id}` - Delete engine [Authorize: Admin]

### Manufacturers
- `GET /api/manufacturer` - List all manufacturers [Authorize: User, Instructor, Admin]
- `GET /api/manufacturer/{id}` - Get manufacturer details [Authorize: User, Instructor, Admin]
- `POST /api/manufacturer` - Create manufacturer [Authorize: Instructor, Admin]
- `PUT /api/manufacturer/{id}` - Update manufacturer [Authorize: Admin]
- `DELETE /api/manufacturer/{id}` - Delete manufacturer [Authorize: Admin]

### Car Profiles
- `GET /api/carprofiles` - List all car profiles [Authorize: User, Instructor, Admin]
- `GET /api/carprofiles/{id}` - Get car profile details [Authorize: User, Instructor, Admin]
- `POST /api/carprofiles` - Create car profile [Authorize: Instructor, Admin]
- `PUT /api/carprofiles/{id}` - Update car profile [Authorize: Admin]
- `DELETE /api/carprofiles/{id}` - Delete car profile [Authorize: Admin]

### Car-Engine Relationships
- `GET /api/carengines` - List all car-engine relationships [Authorize: User, Instructor, Admin]
- `GET /api/carengines/{carId}/{engineId}` - Get specific relationship [Authorize: User, Instructor, Admin]
- `POST /api/carengines` - Create relationship [Authorize: Instructor, Admin]
- `PUT /api/carengines/{carId}/{engineId}` - Update relationship [Authorize: Admin]
- `DELETE /api/carengines/{carId}/{engineId}` - Delete relationship [Authorize: Admin]

## Entity Relationships

### One-to-Many: Manufacturer → Car
- Each manufacturer can have multiple cars
- Configured in `ApplicationDbContext.OnModelCreating()`

### One-to-One: Car ↔ CarProfile
- Each car has one optional profile with additional details
- Each profile belongs to exactly one car
- Foreign key: CarProfile.CarId

### Many-to-Many: Car ↔ Engine (via CarEngine)
- Cars can have multiple engines, engines can be in multiple cars
- Join entity: CarEngine (with composite key: CarId + EngineId)
- Additional data: InstallationDate, Notes

## Data Transfer Objects (DTOs)

All API endpoints use DTOs to maintain clean separation between internal models and API contracts.

### DTO Types
- **Create DTOs**: For POST requests (e.g., CarCreateDto)
- **Update DTOs**: For PUT requests (e.g., CarUpdateDto)
- **List DTOs**: For GET list endpoints with essential fields only (e.g., CarListDto)
- **Details DTOs**: For GET single item endpoints with complete information (e.g., CarDetailsDto)

### Validation
All DTOs use Data Annotations:
```csharp
[Required]
[StringLength(100)]
public string Brand { get; set; } = string.Empty;

[Range(1900, 2100)]
public int Year { get; set; }
```

Invalid requests return HTTP 400 with validation error details.


### Role-Based Authorization
Three roles control access to endpoints:
- **Admin**: Full access to all endpoints (GET, POST, PUT, DELETE)
- **Instructor**: Can read (GET) and create/update (POST, PUT) entities
- **User**: Read-only access (GET only)


Example:
```csharp
[HttpPost("api/[controller]")]
[Authorize(Roles = "Admin,Instructor")]
public async Task<ActionResult<CarDetailsDto>> CreateApi([FromBody] CarCreateDto carDto)
{
    // Only Instructor and Admin can create
}

[HttpPut("api/[controller]/{id}")]
[Authorize(Roles = "Admin")]
public async Task<ActionResult<CarDetailsDto>> UpdateApi(int id, [FromBody] CarUpdateDto carDto)
{
    // Only Admin can update
}
```

### Test Credentials

| Role | Username | Password |
|------|----------|----------|
| Admin | `admin` | `password` |
| Instructor | `instructor` | `password` |
| User | `user` | `password` |

### API Error Responses

- **401 Unauthorized**: User is not authenticated (no valid cookie)
- **403 Forbidden**: User is authenticated but lacks required role for the endpoint

## Hybrid JWT-in-Cookie Authentication

This application uses a hybrid approach combining JWT tokens with HTTP-only cookies. This design provides the best of both worlds: stateless token-based authentication with XSS protection.

### How It Works:
1. Client sends credentials to `POST /api/auth/login`
2. Server validates credentials and generates a JWT token containing user claims (username, role)
3. JWT token is stored in an HTTP-only cookie and returned via `Set-Cookie` header
4. Browser automatically includes the cookie in subsequent API requests
5. Server extracts the JWT from the cookie header, validates the signature, and extracts claims
6. User claims from the JWT are used for authorization decisions

### Security Implementation:
1. **Token Signing**: JWT tokens are signed using HMAC-SHA256 with a server secret
2. **HTTP-Only Flag**: Cookies are marked `HttpOnly`, preventing JavaScript access
3. **Secure Flag**: In production, cookies use `Secure` flag (HTTPS only)
4. **SameSite Policy**: Cookies use `SameSite=Strict` to prevent CSRF attacks
5. **Token Expiration**: JWT tokens expire after 1 hour (configurable in `appsettings.json`)
6. **Signature Validation**: Server validates token signature on every request using the stored secret

### Testing with Swagger:
1. Navigate to `http://localhost:5000/swagger` to open Swagger UI
2. Login via `POST /api/auth/login` with test credentials:
   - **Admin**: username: `admin`, password: `password`
   - **Instructor**: username: `instructor`, password: `password`
   - **User**: username: `user`, password: `password`
3. The server returns a response. The JWT token is automatically stored in an HTTP-only cookie by your browser
4. All subsequent API requests automatically include the cookie with the JWT token
5. Server validates the token signature and extracts claims for authorization
6. Endpoints with role requirements will return:
   - **200 OK**: If user has required role
   - **403 Forbidden**: If user lacks required role
   - **401 Unauthorized**: If token is missing or invalid

### Manual Testing with Postman/cURL:
While Postman can capture cookies, the HTTP-only flag makes it more secure:
1. Send POST request to `http://localhost:5000/api/auth/login`:
   ```json
   {
     "username": "admin",
     "password": "password"
   }
   ```
2. The response includes `Set-Cookie` header with JWT token in HTTP-only cookie
3. Subsequent requests automatically include the cookie due to browser's same-origin policy
4. For testing APIs programmatically, export cookies from Postman or use scripting tools that support cookies


## Why HTTP-Only Cookies are the Industry Standard for Authentication

HTTP-only cookies are widely preferred in traditional web applications for several critical security reasons:

### Security Advantages of HTTP-Only Cookies:

1. **XSS (Cross-Site Scripting) Protection**
   - HTTP-only cookies cannot be accessed via JavaScript (document.cookie)
   - Even if attackers inject malicious scripts, they cannot steal the authentication token
   - JWT tokens in localStorage/sessionStorage are vulnerable to XSS attacks

2. **CSRF (Cross-Site Request Forgery) Protection**
   - Browsers automatically include cookies with same-origin requests
   - CSRF tokens can be validated server-side
   - JWT in headers requires explicit inclusion, adding complexity

3. **XSS Mitigation Without Additional Tokens**
   - No need for separate CSRF tokens with HTTP-only cookies
   - Reduces overall complexity of authentication system

4. **Automatic Token Management**
   - Browsers handle cookie lifecycle automatically
   - No manual token management code needed
   - Reduces implementation errors

5. **Domain and Path Restrictions**
   - Cookies support domain and path scoping
   - Tokens can restrict where they're valid (though more limited than cookies)

## Service Layer Architecture

The project implements three main services with dependency injection:

### ICarService / CarService
- Manages car entity operations
- Uses LINQ projections to map Car → CarListDto/CarDetailsDto
- Implements AsNoTracking() for read operations

### IEngineService / EngineService
- Manages engine entity operations
- Handles validation before database operations

### IManufacturerService / ManufacturerService
- Manages manufacturer entity operations
- Includes car relationships

Service interfaces are registered in `Program.cs`:
```csharp
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IEngineService, EngineService>();
builder.Services.AddScoped<IManufacturerService, ManufacturerService>();
```

## Query Optimization

### LINQ Select Projections
Services use Select() to project entities into DTOs, returning only required fields:
```csharp
public async Task<IEnumerable<CarListDto>> GetAllAsync()
{
    return await _context.Cars
        .Include(c => c.Manufacturer)
        .Select(c => new CarListDto
        {
            Id = c.Id,
            Brand = c.Brand,
            Model = c.Model,
            Year = c.Year,
            ManufacturerName = c.Manufacturer != null ? c.Manufacturer.Name : "Unknown"
        })
        .AsNoTracking()
        .ToListAsync();
}
```

### AsNoTracking() for Read-Only Queries
All SELECT queries use `.AsNoTracking()` to:
- Disable entity change tracking (reduces memory usage)
- Improve query performance
- Prevent accidental modifications of detached entities

## Database Migrations

The project includes Entity Framework Core migrations for schema management:
```bash
# Apply migrations automatically on startup (via Program.cs configuration)
# Or manually apply if needed:
dotnet ef database update
```

Migrations track all schema changes and include seed data for demo purposes.

## Project Structure

```
WebApplication3/
├── Controllers/              # API and MVC endpoints
│   ├── AuthController.cs     # Cookie authentication endpoints
│   ├── CarController.cs      # Car CRUD endpoints
│   ├── EngineController.cs   # Engine CRUD endpoints
│   ├── ManufacturerController.cs
│   ├── CarProfilesController.cs
│   └── CarEnginesController.cs
├── Models/                   # Entity classes
│   ├── Car.cs
│   ├── Engine.cs
│   ├── Manufacturer.cs
│   ├── CarProfile.cs
│   └── CarEngine.cs
├── DTOs/                     # Data Transfer Objects
│   ├── CarCreateDto.cs
│   ├── CarUpdateDto.cs
│   ├── CarListDto.cs
│   ├── CarDetailsDto.cs
│   └── ... (other DTOs)
├── Services/                 # Business logic
│   ├── CarService.cs
│   ├── EngineService.cs
│   ├── ManufacturerService.cs
│   └── ... (other services)
├── Interfaces/               # Service contracts
│   ├── ICarService.cs
│   ├── IEngineService.cs
│   └── ... (other interfaces)
├── Data/
│   └── ApplicationDbContext.cs  # EF Core DbContext
├── Migrations/               # Database schema migrations
├── Views/                    # MVC views (optional)
├── Program.cs                # Application startup configuration
├── appsettings.json          # Configuration file
└── docker-compose.yml        # Docker configuration
```

## Getting Started

### Prerequisites

- **.NET 8.0 SDK** - Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Docker Desktop** - Download from [docker.com](https://www.docker.com/products/docker-desktop) (for SQL Server)
- **Entity Framework Core Tools** - Install with: `dotnet tool install --global dotnet-ef`
- **Swagger** - For testing API endpoints 
### Installation & Setup

1. **Clone the repository** 
   ```bash
   git clone https://github.com/Mo-aziz/Cars-Finder.git
   cd WebApplication3
   ```

2. **Start the SQL Server database**:
   ```bash
   docker-compose up -d
   ```
   This starts SQL Server in Docker on `localhost:1433`.
   - Username: `sa`
   - Password: `YourStrong@Passw0rd`


3. **Apply database migrations** (creates database schema and seed data):
   ```bash
   dotnet ef database update
   ```

4. **Run the application**:
   ```bash
   dotnet run
   ```
  

5. **Access the application**:
   - API Swagger UI: http://localhost:5000/swagger
   - MVC Views: http://localhost:5000/

### Database Seeding

The application automatically seeds demo data on startup:
- **3 Manufacturers**: Toyota, Ford, BMW
- **3 Cars**: Camry, Mustang, X5
- **3 Engines**: V6, V8, Inline-6

No additional setup required.

## Testing the API

### 1. Start the Application
```bash
dotnet run
```

### 2. Get Authentication Token

**Via Swagger UI**:
1. Navigate to http://localhost:5000/swagger/ui/index.html
2. Click POST "/api/auth/login"
3. Click Try it out on the top right
4. Use credentials:
   - Username: `admin`, Password: `password` (Admin role)
   - Username: `user`, Password: `password` (User role)
   - Username: `instructor`, Password: `password` (Instructor role)


### 3. Test Endpoints

#### Create Car (requires Admin or Instructor)
- **Endpoint**: `POST /api/car`
- **Body**:
  ```json
  {
    "brand": "Tesla",
    "model": "Model 3",
    "year": 2023,
    "manufacturerId": 1
  }
  ```
- **Expected**: 201 Created with car details

#### Get All Cars (public)
- **Endpoint**: `GET /api/car`
- **Expected**: 200 OK with list of cars

#### Update Car (requires Admin)
- **Endpoint**: `PUT /api/car/1`
- **Body**:
  ```json
  {
    "brand": "Updated Brand",
    "model": "Updated Model",
    "year": 2024,
    "manufacturerId": 1
  }
  ```

#### Delete Car (requires Admin only)
- **Endpoint**: `DELETE /api/car/1`
- **Expected**: 204 No Content

### 5. Error Responses

#### Validation Error (HTTP 400)
```json
{
  "errors": {
    "Brand": ["The Brand field is required."],
    "Year": ["The field Year must be between 1900 and 2100."]
  }
}
```

#### Unauthorized (HTTP 401)
```json
{
  "message": "Unauthorized"
}
```

#### Not Found (HTTP 404)
```json
{
  "message": "Not found"
}
```

## Code Examples

### Service with AsNoTracking() and LINQ Projection
```csharp
public async Task<IEnumerable<CarListDto>> GetAllAsync()
{
    return await _context.Cars
        .Include(c => c.Manufacturer)
        .Select(c => new CarListDto  // LINQ projection to DTO
        {
            Id = c.Id,
            Brand = c.Brand,
            Model = c.Model,
            Year = c.Year,
            ManufacturerName = c.Manufacturer != null ? c.Manufacturer.Name : "Unknown"
        })
        .AsNoTracking()  // Disable change tracking for read-only queries
        .ToListAsync();  // Async database query
}
```

### Controller with Authorization
```csharp
[HttpPost("api/[controller]")]
[Authorize(Roles = "Admin,Instructor")]  // Only Admin and Instructor roles allowed
public async Task<ActionResult<CarDetailsDto>> CreateApi([FromBody] CarCreateDto carDto)
{
    // Validate DTO before database operation
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
    
    var car = await _carService.CreateAsync(carDto);
    return CreatedAtAction(nameof(GetByIdApi), new { id = car.Id }, car);
}
```

### DTO with Validation Attributes
```csharp
public class CarCreateDto
{
    [Required]
    [StringLength(100)]
    public string Brand { get; set; } = string.Empty;
    
    [Required]
    [Range(1900, 2100)]
    public int Year { get; set; }
}
```

## Troubleshooting

### SQL Server not connecting
```
Error: Connection timeout
```
**Solution**: 
- Confirm Docker is running: `docker ps`
- Check container is healthy: `docker-compose logs`
- Wait 30-60 seconds and retry

### Port 5000 already in use
**Solution**: 
- Change port in `Program.cs`: `builder.WebHost.UseUrls("http://localhost:5001");`
- Or kill process: `netstat -ano | findstr :5000` (Windows)

### Migrations not applying
**Solution**:
```bash
dotnet ef database drop --force
dotnet ef database update
```


## Learning Objectives Addressed

- Entity relationships (one-to-one, one-to-many, many-to-many)
- Dependency injection with database services
- Clean service layer design with interfaces
- DTOs for request/response models
- DTO validation using Data Annotations
- JWT authentication implementation
- Role-based authorization with [Authorize] attributes
- LINQ query optimization with Select() projections
- AsNoTracking() for read-only queries
- Async database operations (ToListAsync, FirstOrDefaultAsync, SaveChangesAsync)
- EF Core migrations
- Swagger API documentation
- HTTP-only cookies explanation

## Screenshots
- GET Manufacturer (Admin Role)
![alt text](<Screenshot 2026-04-05 204227.png>)
- POST Manufacturer (Admin Role)
![alt text](<Screenshot 2026-04-05 204415.png>)
- POST Car (Admin Role)
![alt text](<Screenshot 2026-04-05 204559.png>)
- POST Car (Instructor Role)
![alt text](<Screenshot 2026-04-05 204919.png>)
- GET Car (Admin Role)
![alt text](<Screenshot 2026-04-05 204143.png>)
- PUT Car (Admin Role)
![alt text](<Screenshot 2026-04-05 204713.png>)
- POST Engine(Instructor Role)
![alt text](<Screenshot 2026-04-05 205052.png>)
- PUT Engine (Instructor Role)
![alt text](<Screenshot 2026-04-05 205141.png>)
- DELETE Car (Admin role)
![alt text](<Screenshot 2026-04-05 204821.png>)
- DELETE Car (User role)
![alt text](<Screenshot 2026-04-05 204020.png>)

## Author

- **Mohammed Ahmed AbdelAziz** - [GitHub Profile](https://github.com/Mo-aziz)

## License

This project is open source and available for educational purposes.

---

Built using ASP.NET Core MVC

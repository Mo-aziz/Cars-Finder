# Cars Finder

An ASP.NET Core MVC web application for managing Cars and Engines with in-memory storage.

## Project Overview

This project demonstrates the Model-View-Controller (MVC) design pattern using ASP.NET Core. It includes two main entities - **Car** and **Engine** - each with full CRUD operations (Create, Read operations implemented).

## Features

- **Car Management**: Add, view, and list cars with properties (Id, Brand, Model, Year)
- **Engine Management**: Add, view, and list engines with properties (Id, Type, HorsePower)
- **In-Memory Storage**: Uses static Lists for data persistence during application runtime
- **Clean Architecture**: Separates concerns using Models, Interfaces, Controllers, and Views
- **Responsive UI**: Bootstrap 5 styling

## Project Structure

```
WebApplication3/
├── Controllers/
│   ├── CarController.cs      # Car logic and endpoints
│   └── EngineController.cs     # Engine logic and endpoints
├── Interfaces/
│   ├── ICar.cs                 # Car interface contract
│   └── IEngine.cs                # Engine interface contract
├── Models/
│   ├── Car.cs                    # Car model class
│   └── Engine.cs                 # Engine model class
├── Views/
│   ├── Car/                      # Car views
│   │   ├── Create.cshtml
│   │   ├── Details.cshtml
│   │   └── Index.cshtml
│   ├── Engine/                   # Engine views
│   │   ├── Create.cshtml
│   │   ├── Details.cshtml
│   │   └── Index.cshtml
│   └── Shared/
│       └── _Layout.cshtml        # Master page template
├── Program.cs                    # Application entry point
└── WebApplication3.csproj        # Project file
```

## Technologies Used

- **.NET 8.0** - Framework
- **ASP.NET Core MVC** - Web framework
- **Razor Views** - Templating engine
- **Bootstrap 5** - CSS framework
- **C# 12** - Programming language

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Installation & Running

1. Clone the repository:
   ```bash
   git clone https://github.com/Mo-aziz/Cars-Finder.git
   cd Cars-Finder
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Open your browser and navigate to:
   - http://localhost:5000

### Default Routes

| URL | Description |
|-----|-------------|
| `/Car/Index` | List all cars |
| `/Car/Create` | Add a new car |
| `/Car/Details/{id}` | View car details |
| `/Engine/Index` | List all engines |
| `/Engine/Create` | Add a new engine |
| `/Engine/Details/{id}` | View engine details |

## Architecture Explained

### Models
Define the data structure:
- **Car**: Id, Brand, Model, Year
- **Engine**: Id, Type, HorsePower

### Interfaces
Define the contract (what operations are available):
- **ICar**: GetAll(), GetById(int id), Add(Car car)
- **IEngine**: GetAll(), GetById(int id), Add(Engine engine)

### Controllers
Handle HTTP requests and implement interfaces:
- Inherit from `Controller` base class
- Implement interface methods
- Use `static List<T>` for in-memory storage
- Return appropriate Views

### Views
Razor templates that render HTML:
- **Index.cshtml**: Display list with links to Create and Details
- **Create.cshtml**: Form for adding new items
- **Details.cshtml**: Display single item properties

### Data Flow
```
Browser → Controller → Model (static List) → View → Browser
```

## How to Use

### Adding a Car
1. Click **"Cars"** in the navigation menu
2. Click **"Create New"**
3. Fill in the form (Id, Brand, Model, Year)
4. Click **"Create"**
5. The car appears in the list

### Viewing Car Details
1. From the car list, click **"Details"** link
2. View all properties of that car

### Adding an Engine
1. Click **"Engines"** in the navigation menu
2. Follow the same steps as for cars

## Notes

- **In-Memory Storage**: Data is stored in static Lists and persists only while the application is running. Restarting the app clears all data.
- **No Database**: This project intentionally does not use Entity Framework or any database to keep it simple and focused on MVC concepts.

## Screenshots

*Add screenshots here showing the application in action*

## Future Enhancements

- Add Edit and Delete operations
- Add search/filter functionality
- Implement proper database storage (SQL Server, SQLite)
- Add validation attributes to models
- Add unit tests

## Author

- **Mohammed Aziz** - [GitHub Profile](https://github.com/Mo-aziz)

## License

This project is open source and available for educational purposes.

---

Built with ❤️ using ASP.NET Core MVC

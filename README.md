# Truweather - Full Stack .NET Weather Application

A comprehensive weather application built with .NET 8 featuring real-time weather data, 7-day forecasts, location services, weather alerts, and offline support.

## Project Structure

```
Truweather/
├── TruweatherAPI/     # ASP.NET Core 8.0 backend API
├── TruweatherWeb/     # Blazor web dashboard
├── TruweatherMobile/  # .NET MAUI mobile app (iOS/Android)
└── Truweather.sln     # Solution file
```

## Tech Stack

| Component | Technology |
|-----------|-----------|
| **Backend** | ASP.NET Core 8.0, Entity Framework Core, SQL Server, JWT Auth |
| **Web Frontend** | Blazor Web App, Bootstrap 5 |
| **Mobile** | .NET MAUI (iOS/Android), SQLite |
| **Database** | SQL Server with EF Core migrations |

## Features

### ✅ Completed
- Backend API with JWT authentication
- 5 database models with relationships
- Auth endpoints (register, login, refresh, logout)
- Weather endpoints (current, forecast, locations, alerts)
- Blazor web project scaffold
- MAUI mobile project scaffold

### 🔄 In Development
- Weather API integration (OpenWeatherMap)
- Web dashboard UI
- Mobile app screens
- Offline caching
- Alert notifications

## Prerequisites

- **.NET 8.0 SDK** or higher
- **Visual Studio 2022** or **VS Code** with C# extensions
- **SQL Server Express** (local) or **Azure SQL Database**
- **Git**

## Quick Start

### 1. Restore and Build
```bash
cd Truweather
dotnet restore
dotnet build
```

### 2. Configure Database

Update `TruweatherAPI/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=truweather_dev;Integrated Security=true;Encrypt=false;"
  }
}
```

### 3. Run Development Servers

**Backend API** (Terminal 1)
```bash
cd TruweatherAPI && dotnet run
# Swagger: http://localhost:5000/swagger
```

**Web Dashboard** (Terminal 2)
```bash
cd TruweatherWeb && dotnet run
# App: https://localhost:5001
```

**Mobile** (Terminal 3)
```bash
cd TruweatherMobile
dotnet build -t:Run -f net8.0-ios    # iOS Simulator
dotnet build -t:Run -f net8.0-android # Android Emulator
```

## Database Setup

### For macOS - SQL Server in Docker
```bash
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourPassword@123' \
  -p 1433:1433 -d mcr.microsoft.com/mssql/server
```

Update connection string to use Docker SQL Server:
```
Server=localhost,1433;Database=truweather_dev;User ID=sa;Password=YourPassword@123;Encrypt=false;
```

### Database Migrations

The API automatically applies migrations on startup. To manually manage:

```bash
cd TruweatherAPI

# Create new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Revert last migration
dotnet ef migrations remove
```

## API Endpoints

All endpoints documented in Swagger at `/swagger` when running the API.

### Authentication
- `POST /api/auth/register` - Create new user account
- `POST /api/auth/login` - Login with email/password
- `POST /api/auth/refresh` - Refresh JWT token
- `POST /api/auth/logout` - Logout user

### Weather (Requires Authorization)
- `GET /api/weather/current?latitude=X&longitude=Y` - Current weather
- `GET /api/weather/forecast?latitude=X&longitude=Y` - 7-day forecast
- `GET /api/weather/locations` - Get user's saved locations
- `POST /api/weather/locations` - Add saved location
- `PUT /api/weather/locations/{id}` - Update location
- `DELETE /api/weather/locations/{id}` - Delete location
- `GET /api/weather/alerts` - Get weather alerts
- `POST /api/weather/alerts` - Create alert
- `PUT /api/weather/alerts/{id}` - Update alert
- `DELETE /api/weather/alerts/{id}` - Delete alert

## Configuration

### appsettings.json (API)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=truweather_dev;Integrated Security=true;Encrypt=false;"
  },
  "Jwt": {
    "Secret": "your-secret-key-minimum-32-characters",
    "Issuer": "https://truweather.com",
    "Audience": "truweather-app",
    "ExpirationMinutes": 60
  },
  "WeatherApi": {
    "Provider": "OpenWeatherMap",
    "ApiKey": "your-api-key-here",
    "BaseUrl": "https://api.openweathermap.org"
  }
}
```

## Project Structure Details

```
TruweatherAPI/
├── Controllers/         # API endpoints (Auth, Weather)
├── Models/             # Database entities (User, Location, Weather, Alert, Preferences)
├── Services/           # Business logic (Auth, Weather)
├── DTOs/               # Data transfer objects
├── Data/               # DbContext configuration
├── appsettings.json    # App settings
└── Program.cs          # Startup configuration

TruweatherWeb/
├── Components/         # Blazor components
├── Pages/              # Blazor pages (Home, Counter, Weather)
├── wwwroot/            # Static assets
└── Program.cs          # Startup configuration

TruweatherMobile/
├── Platforms/          # Platform-specific code (iOS/Android)
├── Resources/          # App resources and styles
├── Pages/              # MAUI pages
└── MauiProgram.cs      # Mobile app startup
```

## Important Notes

### JWT Security
- Current default secret in appsettings.json is for **DEVELOPMENT ONLY**
- Must be changed for production (minimum 32 characters)
- Should be stored in Azure Key Vault for production
- Token expires in 60 minutes (configurable)

### Database
- Uses SQL Server-specific decimal precision
- Relationships configured with cascade delete
- Indexed queries on UserId and SavedLocationId for performance
- Ready for migration to production

### CORS
- Currently allows all origins (development only)
- Must be restricted to specific domains in production
- Update in `TruweatherAPI/Program.cs` before deployment

### Weather API Integration
- Currently returns mock data
- Ready to integrate with OpenWeatherMap or WeatherAPI.com
- Service interface prepared in `IWeatherService`

### Mobile Development
Requires .NET workloads installation:

**macOS:**
```bash
dotnet workload install ios android
dotnet workload restore
```

**Windows:**
```bash
dotnet workload install android windows
dotnet workload restore
```

## Troubleshooting

### Database Connection Failed
```bash
# Ensure SQL Server is running
# Check connection string in appsettings.Development.json
# For Docker: verify container is running with: docker ps
```

### Build Errors
```bash
dotnet clean
dotnet restore
dotnet build
```

### Port Already in Use
Update port in `Properties/launchSettings.json` in respective project, or:
```bash
lsof -i :5000  # Find process using port 5000
kill -9 <PID>  # Kill the process
```

### MAUI Build Issues
```bash
# Restore missing workloads
dotnet workload restore

# Clean and rebuild
dotnet clean
dotnet build
```

### Swagger Not Appearing
- Ensure running in development mode
- Check appsettings.Development.json
- Restart the API server

## Next Steps

1. **Integrate Weather API**
   - Get API key from OpenWeatherMap.com or WeatherAPI.com
   - Implement real API calls in `WeatherService`
   - Add caching layer for performance

2. **Develop Web Dashboard**
   - Create Blazor components for weather display
   - Add HttpClient service for API communication
   - Build location management UI
   - Add alert configuration interface

3. **Develop Mobile App**
   - Create MAUI pages for home, weather details, locations
   - Add location permission handling
   - Implement offline support with SQLite
   - Add push notifications for alerts

4. **Testing**
   - Add unit tests for services
   - Add integration tests for API endpoints
   - Add component tests for Blazor pages

5. **CI/CD & Deployment**
   - Set up GitHub Actions workflows
   - Configure Azure App Service deployment
   - Set up database migrations in pipeline
   - Configure environment-specific settings

6. **Production Preparation**
   - Change JWT secret
   - Configure CORS properly
   - Set up logging and monitoring
   - Configure environment variables
   - Add error handling and logging

## GitHub Actions & Azure Deployment

### GitHub Actions Setup
Create `.github/workflows/build.yml` for:
- Build and test on push to main
- Deploy to Azure App Service
- Run database migrations

### Azure Deployment
Requires:
- Azure subscription
- SQL Database
- App Service for API
- App Service for Web
- GitHub Secrets for deployment credentials

### Required GitHub Secrets
```
AZURE_SUBSCRIPTION_ID        # Your Azure subscription ID
AZURE_RESOURCE_GROUP         # Azure resource group name
AZURE_APP_SERVICE_API        # App Service name for API
AZURE_APP_SERVICE_WEB        # App Service name for Web
AZURE_PUBLISH_PROFILE_API    # Download from Azure App Service
AZURE_PUBLISH_PROFILE_WEB    # Download from Azure App Service
```

## Git Configuration

- **.gitignore** - Excludes build artifacts, IDE files, dependencies
- **.gitattributes** - Ensures consistent line endings across platforms

## License

MIT License

## Author

Preetanshu Mishra

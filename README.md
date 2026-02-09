# Truweather - Full Stack .NET Weather Application

A comprehensive weather application built with .NET 10 featuring real-time weather data from Open-Meteo, 7-day forecasts, saved locations, weather alerts, and user preferences across Web and Mobile platforms.

## Project Structure

```
Truweather/
├── TruweatherCore/       # Shared library (DTOs, interfaces, utilities, i18n)
├── TruweatherAPI/        # ASP.NET Core backend API
├── TruweatherWeb/        # Blazor Server web dashboard
├── TruweatherMobile/     # .NET MAUI mobile app (iOS/Android)
├── .github/workflows/    # CI/CD pipeline
├── global.json           # SDK version (10.0.100)
└── Truweather.sln        # Solution file
```

## Tech Stack

| Component | Technology |
|-----------|-----------|
| **Shared Library** | .NET 10.0 (no external dependencies) |
| **Backend API** | ASP.NET Core 10.0, Entity Framework Core 10.0, SQL Server, JWT Auth |
| **Web Frontend** | Blazor Server with Interactive Server rendering |
| **Mobile App** | .NET MAUI 10.0.1, CommunityToolkit.Mvvm 8.4.0 |
| **Weather Data** | Open-Meteo API (free, no API key required) |
| **Database** | SQL Server with EF Core auto-migrations |
| **CI/CD** | GitHub Actions (.NET 10.0.x, ubuntu-latest) |

## Features

- JWT authentication with register, login, token refresh, logout
- Real-time weather data from Open-Meteo (current conditions + 7-day forecast)
- Saved locations with default location support
- Weather alerts with configurable type, condition, and threshold
- User preferences (temperature/wind units, theme, language, notifications)
- Internationalization support (10 languages, fully translated)
- Shared Core library for code reuse across all projects
- CI/CD pipeline with automated build, test, and artifact upload
- API response caching (in-memory + database persistence, configurable TTLs)
- Offline mobile caching (MonkeyCache.FileStore, 60-minute TTL)
- Comprehensive testing (66 tests: 43 unit + 23 integration, xUnit + FluentAssertions)
- Postman collection for API testing (pre-configured with all 14 endpoints)

## Prerequisites

- **.NET 10.0 SDK** (`dotnet --version` should show 10.x)
- **SQL Server** (local, Docker, or Azure SQL Database)
- **Git**

For mobile development:
```bash
dotnet workload install maui
dotnet workload restore
```

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
dotnet build -t:Run -f net10.0-ios      # iOS Simulator
dotnet build -t:Run -f net10.0-android   # Android Emulator
```

## Architecture

### TruweatherCore (Shared Library)

Zero-dependency library shared across API, Web, and Mobile:

```
TruweatherCore/
├── Constants/          # ApiEndpoints, ErrorMessages, ValidationRules
├── Exceptions/         # ApiException, TruweatherException, ValidationException
├── Http/               # HttpClientWrapper, ITokenStorage interface
├── Models/
│   ├── Domain/         # User, SavedLocation, WeatherAlert, UserPreferences, WeatherData
│   └── DTOs/           # Auth, Weather, Preferences data transfer objects
├── Resources/          # i18n for 10 languages (en, es, fr, de, it, pt, ru, zh, ja, ko)
├── Services/Interfaces/  # IAuthService, IWeatherService, IPreferencesService
└── Utilities/          # CoordinateValidator, TemperatureConverter, WindSpeedConverter, DateTimeFormatter
```

### TruweatherAPI (Backend)

ASP.NET Core API with Entity Framework Core and Open-Meteo integration:

```
TruweatherAPI/
├── Controllers/        # AuthController, WeatherController, PreferencesController
├── Models/             # EF entities (User extends IdentityUser, SavedLocation, WeatherAlert, etc.)
├── Services/
│   ├── OpenMeteo/      # OpenMeteoWeatherService, response models, WeatherCodeMapper
│   ├── AuthService.cs
│   ├── WeatherService.cs
│   └── PreferencesService.cs
├── Data/               # TruweatherDbContext
└── Program.cs          # DI, JWT, Identity, Swagger, CORS configuration
```

### TruweatherWeb (Blazor Server)

Interactive Blazor Server app with custom auth state management:

```
TruweatherWeb/
├── Components/
│   ├── Layout/         # MainLayout (auth top bar), NavMenu (conditional nav)
│   ├── Pages/          # Home (dashboard), Login, Register, Locations, Alerts, Settings
│   └── Shared/         # LoadingSpinner, ErrorAlert, ConfirmDialog, WeatherCard, ForecastDayCard
├── Services/           # AuthService, ServerTokenStorage, TruweatherAuthStateProvider
└── Program.cs          # DI with scoped auth services
```

### TruweatherMobile (.NET MAUI)

Cross-platform mobile app with MVVM architecture:

```
TruweatherMobile/
├── Pages/              # LoginPage, RegisterPage, DashboardPage, LocationsPage, AlertsPage, SettingsPage
├── ViewModels/         # One ViewModel per page (ObservableObject + RelayCommand)
├── Services/           # AuthServiceClient, WeatherServiceClient, PreferencesServiceClient, SecureTokenStorage
├── Converters/         # IsNotNull, IsNull, InvertedBool, IsNotZero
├── Platforms/          # iOS (14.2+), Android (21+)
├── AppShell.xaml       # TabBar navigation (Dashboard, Locations, Alerts, Settings) + auth routes
├── MauiProgram.cs      # DI container (services, ViewModels, pages)
└── App.xaml.cs         # Session restore on startup
```

## API Endpoints

### Swagger UI

All endpoints are documented in interactive Swagger UI at `/swagger` when running the API.

### Authentication
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Create new user account |
| POST | `/api/auth/login` | Login with email/password |
| POST | `/api/auth/refresh` | Refresh JWT token |
| POST | `/api/auth/logout` | Logout user |

### Weather (Requires Authorization)
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/weather/current?latitude=X&longitude=Y` | Current weather |
| GET | `/api/weather/forecast?latitude=X&longitude=Y` | 7-day forecast |
| GET | `/api/weather/locations` | Get saved locations |
| POST | `/api/weather/locations` | Add saved location |
| PUT | `/api/weather/locations/{id}` | Update location |
| DELETE | `/api/weather/locations/{id}` | Delete location |
| GET | `/api/weather/alerts` | Get weather alerts |
| POST | `/api/weather/alerts` | Create alert |
| PUT | `/api/weather/alerts/{id}` | Update alert |
| DELETE | `/api/weather/alerts/{id}` | Delete alert |

## Weather API

Uses **Open-Meteo** — a free, open-source weather API requiring no API key or account.

- Current weather: temperature, humidity, wind speed/direction, pressure, weather condition
- 7-day forecast: daily max/min/mean temperatures, precipitation, wind, conditions
- Automatic timezone detection
- Weather codes mapped to human-readable conditions and icons via `WeatherCodeMapper`

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
  }
}
```

No weather API key configuration needed — Open-Meteo is free and keyless.

## CI/CD

GitHub Actions workflow (`.github/workflows/build.yml`):

- **Triggers**: Push/PR to master, main, develop
- **Runner**: ubuntu-latest with .NET 10.0.x
- **Steps**: Checkout, Setup .NET, Restore, Build (Release), Test all 4 projects, Publish API, Upload artifacts (5-day retention)
- **Tests**: API tests are required; Core, Web, Mobile tests continue on error

## Database Setup

### macOS/Linux — SQL Server in Docker
```bash
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourPassword@123' \
  -p 1433:1433 -d mcr.microsoft.com/mssql/server
```

Connection string for Docker:
```
Server=localhost,1433;Database=truweather_dev;User ID=sa;Password=YourPassword@123;Encrypt=false;
```

### Migrations

The API auto-applies migrations on startup. To manage manually:

```bash
cd TruweatherAPI
dotnet ef migrations add MigrationName
dotnet ef database update
dotnet ef migrations remove
```

## Important Notes

### Security
- JWT secret in appsettings.json is for **development only** — change for production
- CORS currently allows all origins — restrict for production
- Token expiration: 60 minutes (configurable)

### Mobile Platforms
- **iOS**: Minimum 14.2
- **Android**: Minimum API 21
- **Windows**: 10.0.17763 (conditional build on Windows only)

### Internationalization
TruweatherCore includes localization resources for 10 languages: English, Spanish, French, German, Italian, Portuguese, Russian, Chinese, Japanese, Korean.

## Testing

The project includes a comprehensive test suite with 66 tests:

- **43 unit tests**: Service layer (AuthService, WeatherService, PreferencesService, OpenMeteoWeatherService)
- **23 integration tests**: API controllers (Auth, Weather, Preferences)
- **Framework**: xUnit + FluentAssertions + Moq
- **Coverage**: Core business logic and API endpoints

Run tests:
```bash
cd TruweatherAPI.Tests
dotnet test
dotnet test -v normal  # Verbose output
```

## Documentation

- **README.md** - This file (project overview and setup)
- **TODO.md** - Project status and roadmap (22/24 phases complete)
- **PROGRESS.md** - Detailed development log
- **TruweatherCore/Resources/README.md** - Localization system guide
- **Archived Documentation** in `docs/archive/`:
  - API_DOCUMENTATION.md - Comprehensive API reference (Swagger is primary)
  - AZURE_DEPLOYMENT_GUIDE.md - Azure deployment planning
  - PHASE_21_OFFLINE_CACHING.md - Offline caching implementation details

## Troubleshooting

### Build Errors
```bash
dotnet clean && dotnet restore && dotnet build
```

### MAUI Build Issues
```bash
dotnet workload restore
dotnet clean && dotnet build
```

### Database Connection Failed
- Ensure SQL Server is running
- Check connection string in `appsettings.Development.json`
- For Docker: verify container is running with `docker ps`

## License

MIT License

## Author

Preetanshu Mishra

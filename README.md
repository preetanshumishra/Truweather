# Truweather - Full Stack .NET Weather Application

A comprehensive weather application built with .NET 8 featuring real-time weather data, 7-day forecasts, location services, weather alerts, and offline support.

## Project Structure

```
Truweather/
├── TruweatherAPI/           # ASP.NET Core backend API
├── TruweatherWeb/           # Blazor web dashboard
├── TruweatherMobile/        # .NET MAUI mobile app (iOS/Android)
├── Truweather.sln           # Solution file
└── global.json              # .NET SDK version (8.0.0)
```

## Technology Stack

### Backend (TruweatherAPI)
- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT with Identity
- **API Integration**: OpenWeatherMap/WeatherAPI
- **Caching**: In-memory cache for weather data

### Web Frontend (TruweatherWeb)
- **Framework**: Blazor Web App (Server + WebAssembly)
- **Styling**: Bootstrap with custom theming
- **State Management**: Blazor cascading parameters + services

### Mobile Frontend (TruweatherMobile)
- **Framework**: .NET MAUI (iOS/Android)
- **Navigation**: MAUI Shell navigation
- **Location**: MAUI Location service + Maps
- **Storage**: SQLite for offline support
- **Styling**: XAML with MAUI ResourceDictionary

## Features

### Current Implementation
- ✅ Project scaffolding and initial setup
- ✅ Database models (User, Location, Weather, Alerts)
- ✅ Entity Framework configuration
- ✅ NuGet dependencies installed

### In Development
- Authentication & Authorization (JWT)
- Weather API endpoints
- Location service integration
- Weather alert system
- Offline caching
- Web dashboard
- Mobile app UI

## Prerequisites

### Development Environment
- **.NET 8.0 SDK** (or higher)
- **Visual Studio 2022** (latest) OR **Visual Studio Code** with C# extensions
- **SQL Server Express** (local development) or **Azure SQL Database**
- **Node.js 18+** (for frontend tooling if needed)

### For Azure Deployment
- **Azure Subscription** (free trial available)
- **Azure CLI** installed locally
- **GitHub account** with this repository

## Local Development Setup

### 1. Clone and Restore

```bash
cd Truweather
dotnet restore
```

### 2. Configure SQL Server

**Option A: Local SQL Server Express**
```bash
# Update connection string in TruweatherAPI/appsettings.Development.json
# Default: Server=.;Database=truweather_dev;Integrated Security=true;
```

**Option B: Azure SQL Database**
```bash
# Update connection string in TruweatherAPI/appsettings.Development.json
# Server=<server>.database.windows.net;Initial Catalog=truweather;User ID=<user>;Password=<password>;
```

### 3. Apply Database Migrations

```bash
cd TruweatherAPI
dotnet ef database update
```

### 4. Run Development Servers

**Terminal 1 - Backend API (http://localhost:5000)**
```bash
cd TruweatherAPI
dotnet run
# Swagger docs: http://localhost:5000/swagger
```

**Terminal 2 - Web Dashboard (http://localhost:5001)**
```bash
cd TruweatherWeb
dotnet run
```

**Terminal 3 - Mobile App**
```bash
cd TruweatherMobile
dotnet build -t:Run -f net8.0-ios    # iOS Simulator
# OR
dotnet build -t:Run -f net8.0-android # Android Emulator
```

## API Endpoints (Swagger Available at `/swagger`)

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login with credentials
- `POST /api/auth/refresh` - Refresh JWT token
- `POST /api/auth/logout` - Logout user

### Weather Data
- `GET /api/weather/current/{latitude}/{longitude}` - Current weather
- `GET /api/weather/forecast/{latitude}/{longitude}` - 7-day forecast
- `GET /api/weather/locations` - User's saved locations
- `POST /api/weather/locations` - Add saved location

### Alerts
- `GET /api/alerts` - Get weather alerts for user
- `POST /api/alerts` - Create weather alert
- `DELETE /api/alerts/{id}` - Delete alert

### User
- `GET /api/user/profile` - Get user profile
- `PUT /api/user/preferences` - Update preferences
- `DELETE /api/user/account` - Delete account

## GitHub Actions CI/CD Pipeline

### Workflow: `.github/workflows/deploy.yml`

Triggers on:
- Push to `main` branch
- Pull requests to `main`

Steps:
1. **Checkout code** - Clone repository
2. **Setup .NET 8** - Install SDK
3. **Restore dependencies** - `dotnet restore`
4. **Build solution** - `dotnet build --configuration Release`
5. **Run tests** - `dotnet test` (when added)
6. **Publish** - `dotnet publish -c Release -o ./publish`
7. **Deploy to Azure** - Push to Azure App Service

### Required GitHub Secrets

```
AZURE_SUBSCRIPTION_ID        # Your Azure subscription ID
AZURE_RESOURCE_GROUP         # Azure resource group name
AZURE_APP_SERVICE_API        # App Service name for API
AZURE_APP_SERVICE_WEB        # App Service name for Web
AZURE_PUBLISH_PROFILE_API    # Download from Azure App Service
AZURE_PUBLISH_PROFILE_WEB    # Download from Azure App Service
```

### Download Publish Profile from Azure

1. Navigate to Azure Portal → App Service → Overview
2. Click "Get publish profile"
3. Save the XML file
4. Add to GitHub Secrets as base64:
   ```bash
   cat publish-profile.xml | base64 | pbcopy
   # Paste in GitHub Secrets
   ```

## Azure Deployment

### Prerequisites
- Azure subscription with resource group
- SQL Database created in Azure
- Two App Service plans (one for API, one for Web)

### Step 1: Create Resource Group

```bash
az group create --name TruweatherRG --location eastus
```

### Step 2: Create SQL Database

```bash
az sql server create \
  --resource-group TruweatherRG \
  --name truweather-server \
  --admin-user truadmin \
  --admin-password YourSecurePassword123!

az sql db create \
  --resource-group TruweatherRG \
  --server truweather-server \
  --name truweather_db \
  --service-objective S0
```

### Step 3: Create App Services

**For API:**
```bash
az appservice plan create \
  --name TruweatherAPIPlan \
  --resource-group TruweatherRG \
  --sku B1 --is-linux

az webapp create \
  --resource-group TruweatherRG \
  --plan TruweatherAPIplan \
  --name truweather-api \
  --runtime "DOTNETCORE|8.0"
```

**For Web:**
```bash
az appservice plan create \
  --name TruweatherWebPlan \
  --resource-group TruweatherRG \
  --sku B1 --is-linux

az webapp create \
  --resource-group TruweatherRG \
  --plan TruweatherWebPlan \
  --name truweather-web \
  --runtime "DOTNETCORE|8.0"
```

### Step 4: Configure Connection Strings

```bash
# For API
az webapp config connection-string set \
  --resource-group TruweatherRG \
  --name truweather-api \
  --settings TruweatherDb="Server=tcp:truweather-server.database.windows.net,1433;Initial Catalog=truweather_db;..." \
  --connection-string-type SQLServer
```

### Step 5: Configure Environment Variables

```bash
az webapp config appsettings set \
  --resource-group TruweatherRG \
  --name truweather-api \
  --settings \
    JwtSecret="your-secret-key" \
    WeatherApiKey="your-openweathermap-key" \
    AllowedOrigins="https://truweather-web.azurewebsites.net"
```

### Step 6: Push to GitHub and Trigger Deployment

```bash
git add .
git commit -m "Initial Truweather project setup"
git push origin main
```

GitHub Actions will automatically build and deploy both API and Web.

## Testing

### Run All Tests
```bash
dotnet test Truweather.sln
```

### Run Specific Project Tests
```bash
dotnet test TruweatherAPI/TruweatherAPI.csproj
dotnet test TruweatherWeb/TruweatherWeb.csproj
```

### With Coverage
```bash
dotnet test Truweather.sln --collect:"XPlat Code Coverage"
```

## Database Migrations

### Create New Migration
```bash
cd TruweatherAPI
dotnet ef migrations add AddNewFeature
```

### Update Database
```bash
dotnet ef database update
```

### Revert Last Migration
```bash
dotnet ef migrations remove
```

## Configuration

### appsettings.json Structure
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=truweather_dev;Integrated Security=true;"
  },
  "Jwt": {
    "Secret": "your-secret-key-min-32-chars",
    "Issuer": "https://truweather.com",
    "Audience": "truweather-app",
    "ExpirationMinutes": 60
  },
  "WeatherApi": {
    "Provider": "OpenWeatherMap",
    "ApiKey": "your-api-key",
    "BaseUrl": "https://api.openweathermap.org"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

## Troubleshooting

### Connection String Issues
- Verify SQL Server is running: `sqlcmd -S . -Q "SELECT @@VERSION"`
- Check connection in appsettings: `"Server=.;Database=truweather_dev;..."`
- For Azure: Ensure firewall allows your IP

### Migration Errors
```bash
# Clear pending migrations
dotnet ef migrations remove --force

# Recreate from scratch
dotnet ef database drop
dotnet ef database update
```

### GitHub Actions Deployment Failures
1. Check Azure credentials in GitHub Secrets
2. Verify publish profile format (base64 encoded)
3. Ensure app names match configuration
4. Check Azure quota/limits

## Project Roadmap

- [x] Project scaffolding
- [ ] Database schema and models
- [ ] JWT authentication
- [ ] Weather API integration
- [ ] Alert system
- [ ] Web dashboard
- [ ] Mobile app UI
- [ ] Offline caching
- [ ] Unit tests
- [ ] Integration tests
- [ ] Azure deployment
- [ ] CI/CD pipeline

## Environment Variables

Create `.env` files in each project (never commit):

**TruweatherAPI/.env**
```
ConnectionStrings__DefaultConnection=Server=.;Database=truweather_dev;Integrated Security=true;
JwtSecret=your-secret-key-min-32-chars
WeatherApiKey=your-openweathermap-api-key
AllowedOrigins=http://localhost:5001,http://localhost:5002
```

## Contributing

1. Create feature branch: `git checkout -b feature/feature-name`
2. Commit changes: `git commit -m "Brief description"`
3. Push to branch: `git push origin feature/feature-name`
4. Open Pull Request

## License

MIT License - Feel free to use for personal or commercial projects.

## Support

For issues, feature requests, or documentation improvements, please open an issue on GitHub.

---

**Last Updated**: November 2024
**Maintainer**: Preetanshu Mishra

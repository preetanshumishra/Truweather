# Truweather Localization Resources

## Overview

The Resources system provides centralized, multi-language string management for the Truweather application across API, Web, and Mobile platforms.

## Supported Languages

- English (en) - ✅ Complete
- Spanish (es) - ✅ Complete
- French (fr) - ✅ Complete
- German (de) - Placeholder (fallback to English)
- Italian (it) - Placeholder (fallback to English)
- Portuguese (pt) - Placeholder (fallback to English)
- Russian (ru) - Placeholder (fallback to English)
- Chinese (zh) - Placeholder (fallback to English)
- Japanese (ja) - Placeholder (fallback to English)
- Korean (ko) - Placeholder (fallback to English)

## Usage

### Basic Usage

```csharp
using TruweatherCore.Resources;

// Get a localized string
string message = ResourceManager.GetString("auth_login_button");

// Set current language
ResourceManager.SetLanguage("es");

// Get with string interpolation
string greeting = ResourceManager.GetString("dashboard_last_updated", ("time", "2:30 PM"));

// Check available languages
var languages = ResourceManager.GetAvailableLanguages();

// Get current language
string currentLang = ResourceManager.GetCurrentLanguage();
```

### In Blazor Components

```csharp
@page "/dashboard"
@using TruweatherCore.Resources

<h1>@ResourceManager.GetString("dashboard_title")</h1>
<button>@ResourceManager.GetString("auth_login_button")</button>
```

### In MAUI

```csharp
using TruweatherCore.Resources;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        LoginButton.Text = ResourceManager.GetString("auth_login_button");
    }
}

// In XAML with code-behind binding
<Button Text="{Binding LoginButtonText}" />
```

## Adding New Strings

1. Add the key and value to `EnglishResources.cs`
2. Add translations to other language resource files
3. Use the key with `ResourceManager.GetString(key)`

Example:
```csharp
// In EnglishResources.cs
["new_feature_title"] = "New Feature",

// In usage
string title = ResourceManager.GetString("new_feature_title");
```

## String Keys Organization

Strings are organized by feature with prefixes:

- `app_*` - Application-level strings
- `auth_*` - Authentication related
- `weather_*` - Weather data display
- `location_*` - Location management
- `alert_*` - Weather alerts
- `preferences_*` - User preferences
- `unit_*` - Measurement units
- `theme_*` - Theme selections
- `language_*` - Language names
- `nav_*` - Navigation labels
- `message_*` - General messages
- `dashboard_*` - Dashboard specific
- `validation_*` - Validation error messages

## String Interpolation

Strings with placeholders use curly braces:

```csharp
["dashboard_last_updated"] = "Last updated: {time}",

// Usage
string result = ResourceManager.GetString("dashboard_last_updated",
    ("time", "2:30 PM"));
// Result: "Last updated: 2:30 PM"
```

## Fallback Behavior

1. If string not found in current language → fallback to English
2. If current language is English and key not found → return the key itself
3. This ensures the app never breaks, always shows something

## Adding New Languages

1. Create a new resource file: `LanguageNameResources.cs`
2. Implement `GetResources()` method returning Dictionary<string, string>
3. Register in `ResourceManager.InitializeResources()`:
   ```csharp
   _resources["xx"] = LanguageNameResources.GetResources();
   ```

## Placeholder Implementation

Currently, languages other than English and Spanish/French are placeholders that fall back to English. To complete translations:

1. Replace `return EnglishResources.GetResources();` with full translations
2. Or use a translation service/API to populate strings

## Best Practices

- Keep string keys consistent and descriptive
- Use prefixes for organization
- Avoid hardcoding UI strings - always use ResourceManager
- Keep translations close in meaning to English original
- Use placeholders for dynamic content (dates, names, etc.)
- Test UI in multiple languages to verify layout works

## String Count

- **Total unique keys**: 120+
- **Categories**: 13
- **Complete translations**: English, Spanish, French
- **Placeholder translations**: German, Italian, Portuguese, Russian, Chinese, Japanese, Korean

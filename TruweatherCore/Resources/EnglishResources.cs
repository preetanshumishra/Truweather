namespace TruweatherCore.Resources;

/// <summary>
/// English (en) language resource strings for Truweather application.
/// </summary>
public static class EnglishResources
{
    public static Dictionary<string, string> GetResources()
    {
        return new Dictionary<string, string>
        {
            // General
            ["app_name"] = "Truweather",
            ["app_description"] = "Real-time weather tracking and forecasting",
            ["ok"] = "OK",
            ["cancel"] = "Cancel",
            ["save"] = "Save",
            ["delete"] = "Delete",
            ["edit"] = "Edit",
            ["close"] = "Close",
            ["loading"] = "Loading...",
            ["error"] = "Error",
            ["success"] = "Success",
            ["warning"] = "Warning",

            // Authentication
            ["auth_register"] = "Register",
            ["auth_login"] = "Login",
            ["auth_logout"] = "Logout",
            ["auth_email"] = "Email",
            ["auth_password"] = "Password",
            ["auth_confirm_password"] = "Confirm Password",
            ["auth_full_name"] = "Full Name",
            ["auth_register_title"] = "Create Account",
            ["auth_login_title"] = "Login to Truweather",
            ["auth_register_button"] = "Register",
            ["auth_login_button"] = "Login",
            ["auth_forgot_password"] = "Forgot Password?",
            ["auth_already_have_account"] = "Already have an account? Login",
            ["auth_no_account"] = "Don't have an account? Register",
            ["auth_registration_successful"] = "Registration successful! Please login.",
            ["auth_login_successful"] = "Welcome back!",
            ["auth_logout_successful"] = "Logged out successfully",
            ["auth_invalid_credentials"] = "Invalid email or password",
            ["auth_email_already_registered"] = "Email is already registered",
            ["auth_password_mismatch"] = "Passwords do not match",
            ["auth_session_expired"] = "Your session has expired. Please login again.",

            // Weather
            ["weather_current"] = "Current Weather",
            ["weather_forecast"] = "7-Day Forecast",
            ["weather_temperature"] = "Temperature",
            ["weather_feels_like"] = "Feels Like",
            ["weather_condition"] = "Condition",
            ["weather_humidity"] = "Humidity",
            ["weather_wind_speed"] = "Wind Speed",
            ["weather_wind_direction"] = "Wind Direction",
            ["weather_pressure"] = "Pressure",
            ["weather_visibility"] = "Visibility",
            ["weather_uv_index"] = "UV Index",
            ["weather_precipitation"] = "Precipitation",
            ["weather_cloudiness"] = "Cloudiness",
            ["weather_sunrise"] = "Sunrise",
            ["weather_sunset"] = "Sunset",

            // Locations
            ["location_saved_locations"] = "Saved Locations",
            ["location_add_location"] = "Add Location",
            ["location_edit_location"] = "Edit Location",
            ["location_delete_location"] = "Delete Location",
            ["location_location_name"] = "Location Name",
            ["location_latitude"] = "Latitude",
            ["location_longitude"] = "Longitude",
            ["location_set_as_default"] = "Set as Default",
            ["location_default"] = "Default",
            ["location_no_locations"] = "No saved locations. Add one to get started.",
            ["location_added"] = "Location added successfully",
            ["location_updated"] = "Location updated successfully",
            ["location_deleted"] = "Location deleted successfully",
            ["location_invalid_coordinates"] = "Invalid coordinates. Latitude must be between -90 and 90, longitude between -180 and 180.",
            ["location_confirm_delete"] = "Are you sure you want to delete this location?",

            // Alerts
            ["alert_weather_alerts"] = "Weather Alerts",
            ["alert_create_alert"] = "Create Alert",
            ["alert_edit_alert"] = "Edit Alert",
            ["alert_delete_alert"] = "Delete Alert",
            ["alert_alert_type"] = "Alert Type",
            ["alert_condition"] = "Condition",
            ["alert_threshold"] = "Threshold",
            ["alert_enabled"] = "Enabled",
            ["alert_disabled"] = "Disabled",
            ["alert_no_alerts"] = "No weather alerts set",
            ["alert_created"] = "Alert created successfully",
            ["alert_updated"] = "Alert updated successfully",
            ["alert_deleted"] = "Alert deleted successfully",
            ["alert_confirm_delete"] = "Are you sure you want to delete this alert?",

            // Alert Types
            ["alert_type_temperature"] = "Temperature",
            ["alert_type_wind_speed"] = "Wind Speed",
            ["alert_type_humidity"] = "Humidity",
            ["alert_type_pressure"] = "Pressure",
            ["alert_type_precipitation"] = "Precipitation",

            // Alert Conditions
            ["alert_condition_above"] = "Above",
            ["alert_condition_below"] = "Below",
            ["alert_condition_equals"] = "Equals",

            // Preferences
            ["preferences_user_preferences"] = "Preferences",
            ["preferences_temperature_unit"] = "Temperature Unit",
            ["preferences_wind_speed_unit"] = "Wind Speed Unit",
            ["preferences_theme"] = "Theme",
            ["preferences_language"] = "Language",
            ["preferences_notifications"] = "Enable Notifications",
            ["preferences_email_alerts"] = "Enable Email Alerts",
            ["preferences_update_frequency"] = "Update Frequency (minutes)",
            ["preferences_saved"] = "Preferences saved successfully",

            // Units
            ["unit_celsius"] = "Celsius (°C)",
            ["unit_fahrenheit"] = "Fahrenheit (°F)",
            ["unit_kelvin"] = "Kelvin (K)",
            ["unit_ms"] = "Meters per second (m/s)",
            ["unit_kmh"] = "Kilometers per hour (km/h)",
            ["unit_mph"] = "Miles per hour (mph)",
            ["unit_knots"] = "Knots (kts)",

            // Themes
            ["theme_light"] = "Light",
            ["theme_dark"] = "Dark",

            // Languages
            ["language_english"] = "English",
            ["language_spanish"] = "Español (Spanish)",
            ["language_french"] = "Français (French)",
            ["language_german"] = "Deutsch (German)",
            ["language_italian"] = "Italiano (Italian)",
            ["language_portuguese"] = "Português (Portuguese)",
            ["language_russian"] = "Русский (Russian)",
            ["language_chinese"] = "中文 (Chinese)",
            ["language_japanese"] = "日本語 (Japanese)",
            ["language_korean"] = "한국어 (Korean)",

            // Navigation
            ["nav_dashboard"] = "Dashboard",
            ["nav_locations"] = "Locations",
            ["nav_alerts"] = "Alerts",
            ["nav_preferences"] = "Preferences",
            ["nav_profile"] = "Profile",

            // Messages
            ["message_no_data"] = "No data available",
            ["message_try_again"] = "Try Again",
            ["message_network_error"] = "Network error. Please check your connection.",
            ["message_server_error"] = "Server error. Please try again later.",
            ["message_request_timeout"] = "Request timed out. Please try again.",
            ["message_unauthorized"] = "Unauthorized. Please login.",
            ["message_forbidden"] = "Access denied.",
            ["message_not_found"] = "Resource not found.",
            ["message_bad_request"] = "Invalid request. Please check your input.",
            ["message_conflict"] = "Resource already exists.",
            ["message_are_you_sure"] = "Are you sure?",
            ["message_confirmation_required"] = "Confirmation required",

            // Dashboard
            ["dashboard_title"] = "Dashboard",
            ["dashboard_current_location"] = "Current Location",
            ["dashboard_no_default_location"] = "No default location set. Add a location to get started.",
            ["dashboard_last_updated"] = "Last updated: {time}",
            ["dashboard_weather_data_unavailable"] = "Weather data currently unavailable",

            // Validation
            ["validation_required"] = "This field is required",
            ["validation_email_invalid"] = "Invalid email address",
            ["validation_password_too_short"] = "Password must be at least 8 characters",
            ["validation_location_name_required"] = "Location name is required",
            ["validation_coordinates_invalid"] = "Invalid coordinates",
            ["validation_temperature_invalid"] = "Temperature out of valid range",
            ["validation_wind_speed_invalid"] = "Wind speed out of valid range",
            ["validation_update_frequency_invalid"] = "Update frequency must be between 5 and 1440 minutes",
        };
    }
}

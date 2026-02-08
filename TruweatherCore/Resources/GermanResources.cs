namespace TruweatherCore.Resources;

/// <summary>
/// German (de) language resource strings for Truweather application.
/// </summary>
public static class GermanResources
{
    public static Dictionary<string, string> GetResources()
    {
        return new Dictionary<string, string>
        {
            // Allgemein
            ["app_name"] = "Truweather",
            ["app_description"] = "Echtzeit-Wetterverfolgung und -vorhersage",
            ["ok"] = "OK",
            ["cancel"] = "Abbrechen",
            ["save"] = "Speichern",
            ["delete"] = "Löschen",
            ["edit"] = "Bearbeiten",
            ["close"] = "Schließen",
            ["loading"] = "Lädt...",
            ["error"] = "Fehler",
            ["success"] = "Erfolg",
            ["warning"] = "Warnung",

            // Authentifizierung
            ["auth_register"] = "Registrieren",
            ["auth_login"] = "Anmelden",
            ["auth_logout"] = "Abmelden",
            ["auth_email"] = "E-Mail",
            ["auth_password"] = "Passwort",
            ["auth_confirm_password"] = "Passwort bestätigen",
            ["auth_full_name"] = "Vollständiger Name",
            ["auth_register_title"] = "Konto erstellen",
            ["auth_login_title"] = "Bei Truweather anmelden",
            ["auth_register_button"] = "Registrieren",
            ["auth_login_button"] = "Anmelden",
            ["auth_forgot_password"] = "Passwort vergessen?",
            ["auth_already_have_account"] = "Haben Sie bereits ein Konto? Anmelden",
            ["auth_no_account"] = "Haben Sie kein Konto? Registrieren",
            ["auth_registration_successful"] = "Registrierung erfolgreich! Bitte melden Sie sich an.",
            ["auth_login_successful"] = "Willkommen zurück!",
            ["auth_logout_successful"] = "Erfolgreich abgemeldet",
            ["auth_invalid_credentials"] = "Ungültige E-Mail oder Passwort",
            ["auth_email_already_registered"] = "E-Mail ist bereits registriert",
            ["auth_password_mismatch"] = "Passwörter stimmen nicht überein",
            ["auth_session_expired"] = "Ihre Sitzung ist abgelaufen. Bitte melden Sie sich erneut an.",

            // Wetter
            ["weather_current"] = "Aktuelles Wetter",
            ["weather_forecast"] = "7-Tage-Vorhersage",
            ["weather_temperature"] = "Temperatur",
            ["weather_feels_like"] = "Gefühlte Temperatur",
            ["weather_condition"] = "Bedingung",
            ["weather_humidity"] = "Luftfeuchtigkeit",
            ["weather_wind_speed"] = "Windgeschwindigkeit",
            ["weather_wind_direction"] = "Windrichtung",
            ["weather_pressure"] = "Luftdruck",
            ["weather_visibility"] = "Sichtweite",
            ["weather_uv_index"] = "UV-Index",
            ["weather_precipitation"] = "Niederschlag",
            ["weather_cloudiness"] = "Bewölkung",
            ["weather_sunrise"] = "Sonnenaufgang",
            ["weather_sunset"] = "Sonnenuntergang",

            // Orte
            ["location_saved_locations"] = "Gespeicherte Orte",
            ["location_add_location"] = "Ort hinzufügen",
            ["location_edit_location"] = "Ort bearbeiten",
            ["location_delete_location"] = "Ort löschen",
            ["location_location_name"] = "Ortsname",
            ["location_latitude"] = "Breitengrad",
            ["location_longitude"] = "Längengrad",
            ["location_set_as_default"] = "Als Standard festlegen",
            ["location_default"] = "Standard",
            ["location_no_locations"] = "Keine gespeicherten Orte. Fügen Sie einen hinzu, um zu beginnen.",
            ["location_added"] = "Ort erfolgreich hinzugefügt",
            ["location_updated"] = "Ort erfolgreich aktualisiert",
            ["location_deleted"] = "Ort erfolgreich gelöscht",
            ["location_invalid_coordinates"] = "Ungültige Koordinaten. Der Breitengrad muss zwischen -90 und 90, der Längengrad zwischen -180 und 180 liegen.",
            ["location_confirm_delete"] = "Möchten Sie diesen Ort wirklich löschen?",

            // Benachrichtigungen
            ["alert_weather_alerts"] = "Wetterwarnungen",
            ["alert_create_alert"] = "Warnung erstellen",
            ["alert_edit_alert"] = "Warnung bearbeiten",
            ["alert_delete_alert"] = "Warnung löschen",
            ["alert_alert_type"] = "Warnungstyp",
            ["alert_condition"] = "Bedingung",
            ["alert_threshold"] = "Schwellenwert",
            ["alert_enabled"] = "Aktiviert",
            ["alert_disabled"] = "Deaktiviert",
            ["alert_no_alerts"] = "Keine Wetterwarnungen gesetzt",
            ["alert_created"] = "Warnung erfolgreich erstellt",
            ["alert_updated"] = "Warnung erfolgreich aktualisiert",
            ["alert_deleted"] = "Warnung erfolgreich gelöscht",
            ["alert_confirm_delete"] = "Möchten Sie diese Warnung wirklich löschen?",

            // Warnungstypen
            ["alert_type_temperature"] = "Temperatur",
            ["alert_type_wind_speed"] = "Windgeschwindigkeit",
            ["alert_type_humidity"] = "Luftfeuchtigkeit",
            ["alert_type_pressure"] = "Luftdruck",
            ["alert_type_precipitation"] = "Niederschlag",

            // Warnungsbedingungen
            ["alert_condition_above"] = "Oben",
            ["alert_condition_below"] = "Unten",
            ["alert_condition_equals"] = "Gleich",

            // Voreinstellungen
            ["preferences_user_preferences"] = "Voreinstellungen",
            ["preferences_temperature_unit"] = "Temperatureinheit",
            ["preferences_wind_speed_unit"] = "Windgeschwindigkeitseinheit",
            ["preferences_theme"] = "Thema",
            ["preferences_language"] = "Sprache",
            ["preferences_notifications"] = "Benachrichtigungen aktivieren",
            ["preferences_email_alerts"] = "E-Mail-Warnungen aktivieren",
            ["preferences_update_frequency"] = "Aktualisierungshäufigkeit (Minuten)",
            ["preferences_saved"] = "Voreinstellungen erfolgreich gespeichert",

            // Einheiten
            ["unit_celsius"] = "Celsius (°C)",
            ["unit_fahrenheit"] = "Fahrenheit (°F)",
            ["unit_kelvin"] = "Kelvin (K)",
            ["unit_ms"] = "Meter pro Sekunde (m/s)",
            ["unit_kmh"] = "Kilometer pro Stunde (km/h)",
            ["unit_mph"] = "Meilen pro Stunde (mph)",
            ["unit_knots"] = "Knoten (kts)",

            // Themen
            ["theme_light"] = "Hell",
            ["theme_dark"] = "Dunkel",

            // Sprachen
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
            ["nav_locations"] = "Orte",
            ["nav_alerts"] = "Warnungen",
            ["nav_preferences"] = "Voreinstellungen",
            ["nav_profile"] = "Profil",

            // Nachrichten
            ["message_no_data"] = "Keine Daten verfügbar",
            ["message_try_again"] = "Erneut versuchen",
            ["message_network_error"] = "Netzwerkfehler. Bitte überprüfen Sie Ihre Verbindung.",
            ["message_server_error"] = "Serverfehler. Bitte versuchen Sie es später erneut.",
            ["message_request_timeout"] = "Anfrage abgelaufen. Bitte versuchen Sie es erneut.",
            ["message_unauthorized"] = "Nicht autorisiert. Bitte melden Sie sich an.",
            ["message_forbidden"] = "Zugriff verweigert.",
            ["message_not_found"] = "Ressource nicht gefunden.",
            ["message_bad_request"] = "Ungültige Anfrage. Bitte überprüfen Sie Ihre Eingabe.",
            ["message_conflict"] = "Ressource existiert bereits.",
            ["message_are_you_sure"] = "Sind Sie sicher?",
            ["message_confirmation_required"] = "Bestätigung erforderlich",

            // Dashboard
            ["dashboard_title"] = "Dashboard",
            ["dashboard_current_location"] = "Aktueller Ort",
            ["dashboard_no_default_location"] = "Kein Standardort gesetzt. Fügen Sie einen Ort hinzu, um zu beginnen.",
            ["dashboard_last_updated"] = "Zuletzt aktualisiert: {time}",
            ["dashboard_weather_data_unavailable"] = "Wetterdaten derzeit nicht verfügbar",

            // Validierung
            ["validation_required"] = "Dieses Feld ist erforderlich",
            ["validation_email_invalid"] = "Ungültige E-Mail-Adresse",
            ["validation_password_too_short"] = "Passwort muss mindestens 8 Zeichen lang sein",
            ["validation_location_name_required"] = "Ortsname ist erforderlich",
            ["validation_coordinates_invalid"] = "Ungültige Koordinaten",
            ["validation_temperature_invalid"] = "Temperatur außerhalb des gültigen Bereichs",
            ["validation_wind_speed_invalid"] = "Windgeschwindigkeit außerhalb des gültigen Bereichs",
            ["validation_update_frequency_invalid"] = "Aktualisierungshäufigkeit muss zwischen 5 und 1440 Minuten liegen",
        };
    }
}

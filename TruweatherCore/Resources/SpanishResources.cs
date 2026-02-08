namespace TruweatherCore.Resources;

/// <summary>
/// Spanish (es) language resource strings for Truweather application.
/// </summary>
public static class SpanishResources
{
    public static Dictionary<string, string> GetResources()
    {
        return new Dictionary<string, string>
        {
            // General
            ["app_name"] = "Truweather",
            ["app_description"] = "Seguimiento de clima en tiempo real y pronóstico",
            ["ok"] = "OK",
            ["cancel"] = "Cancelar",
            ["save"] = "Guardar",
            ["delete"] = "Eliminar",
            ["edit"] = "Editar",
            ["close"] = "Cerrar",
            ["loading"] = "Cargando...",
            ["error"] = "Error",
            ["success"] = "Éxito",
            ["warning"] = "Advertencia",

            // Authentication
            ["auth_register"] = "Registrarse",
            ["auth_login"] = "Iniciar sesión",
            ["auth_logout"] = "Cerrar sesión",
            ["auth_email"] = "Correo electrónico",
            ["auth_password"] = "Contraseña",
            ["auth_confirm_password"] = "Confirmar contraseña",
            ["auth_full_name"] = "Nombre completo",
            ["auth_register_title"] = "Crear cuenta",
            ["auth_login_title"] = "Iniciar sesión en Truweather",
            ["auth_register_button"] = "Registrarse",
            ["auth_login_button"] = "Iniciar sesión",
            ["auth_forgot_password"] = "¿Olvidó su contraseña?",
            ["auth_already_have_account"] = "¿Ya tiene una cuenta? Iniciar sesión",
            ["auth_no_account"] = "¿No tienes cuenta? Registrarse",
            ["auth_registration_successful"] = "¡Registro exitoso! Por favor inicie sesión.",
            ["auth_login_successful"] = "¡Bienvenido de nuevo!",
            ["auth_logout_successful"] = "Sesión cerrada correctamente",
            ["auth_invalid_credentials"] = "Correo electrónico o contraseña inválidos",
            ["auth_email_already_registered"] = "El correo electrónico ya está registrado",
            ["auth_password_mismatch"] = "Las contraseñas no coinciden",
            ["auth_session_expired"] = "Su sesión ha expirado. Por favor inicie sesión de nuevo.",

            // Weather
            ["weather_current"] = "Clima actual",
            ["weather_forecast"] = "Pronóstico de 7 días",
            ["weather_temperature"] = "Temperatura",
            ["weather_feels_like"] = "Se siente como",
            ["weather_condition"] = "Condición",
            ["weather_humidity"] = "Humedad",
            ["weather_wind_speed"] = "Velocidad del viento",
            ["weather_wind_direction"] = "Dirección del viento",
            ["weather_pressure"] = "Presión",
            ["weather_visibility"] = "Visibilidad",
            ["weather_uv_index"] = "Índice UV",
            ["weather_precipitation"] = "Precipitación",
            ["weather_cloudiness"] = "Nubosidad",
            ["weather_sunrise"] = "Salida del sol",
            ["weather_sunset"] = "Puesta de sol",

            // Locations
            ["location_saved_locations"] = "Ubicaciones guardadas",
            ["location_add_location"] = "Añadir ubicación",
            ["location_edit_location"] = "Editar ubicación",
            ["location_delete_location"] = "Eliminar ubicación",
            ["location_location_name"] = "Nombre de la ubicación",
            ["location_latitude"] = "Latitud",
            ["location_longitude"] = "Longitud",
            ["location_set_as_default"] = "Establecer como predeterminado",
            ["location_default"] = "Predeterminado",
            ["location_no_locations"] = "Sin ubicaciones guardadas. Añada una para empezar.",
            ["location_added"] = "Ubicación añadida exitosamente",
            ["location_updated"] = "Ubicación actualizada exitosamente",
            ["location_deleted"] = "Ubicación eliminada exitosamente",
            ["location_invalid_coordinates"] = "Coordenadas inválidas. La latitud debe estar entre -90 y 90, la longitud entre -180 y 180.",
            ["location_confirm_delete"] = "¿Está seguro de que desea eliminar esta ubicación?",

            // Alerts
            ["alert_weather_alerts"] = "Alertas climáticas",
            ["alert_create_alert"] = "Crear alerta",
            ["alert_edit_alert"] = "Editar alerta",
            ["alert_delete_alert"] = "Eliminar alerta",
            ["alert_alert_type"] = "Tipo de alerta",
            ["alert_condition"] = "Condición",
            ["alert_threshold"] = "Umbral",
            ["alert_enabled"] = "Activado",
            ["alert_disabled"] = "Desactivado",
            ["alert_no_alerts"] = "Sin alertas climáticas establecidas",
            ["alert_created"] = "Alerta creada exitosamente",
            ["alert_updated"] = "Alerta actualizada exitosamente",
            ["alert_deleted"] = "Alerta eliminada exitosamente",
            ["alert_confirm_delete"] = "¿Está seguro de que desea eliminar esta alerta?",

            // Alert Types
            ["alert_type_temperature"] = "Temperatura",
            ["alert_type_wind_speed"] = "Velocidad del viento",
            ["alert_type_humidity"] = "Humedad",
            ["alert_type_pressure"] = "Presión",
            ["alert_type_precipitation"] = "Precipitación",

            // Alert Conditions
            ["alert_condition_above"] = "Por encima de",
            ["alert_condition_below"] = "Por debajo de",
            ["alert_condition_equals"] = "Igual a",

            // Preferences
            ["preferences_user_preferences"] = "Preferencias",
            ["preferences_temperature_unit"] = "Unidad de temperatura",
            ["preferences_wind_speed_unit"] = "Unidad de velocidad del viento",
            ["preferences_theme"] = "Tema",
            ["preferences_language"] = "Idioma",
            ["preferences_notifications"] = "Habilitar notificaciones",
            ["preferences_email_alerts"] = "Habilitar alertas por correo",
            ["preferences_update_frequency"] = "Frecuencia de actualización (minutos)",
            ["preferences_saved"] = "Preferencias guardadas exitosamente",

            // Units
            ["unit_celsius"] = "Celsius (°C)",
            ["unit_fahrenheit"] = "Fahrenheit (°F)",
            ["unit_kelvin"] = "Kelvin (K)",
            ["unit_ms"] = "Metros por segundo (m/s)",
            ["unit_kmh"] = "Kilómetros por hora (km/h)",
            ["unit_mph"] = "Millas por hora (mph)",
            ["unit_knots"] = "Nudos (kts)",

            // Themes
            ["theme_light"] = "Claro",
            ["theme_dark"] = "Oscuro",

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
            ["nav_dashboard"] = "Panel de control",
            ["nav_locations"] = "Ubicaciones",
            ["nav_alerts"] = "Alertas",
            ["nav_preferences"] = "Preferencias",
            ["nav_profile"] = "Perfil",

            // Messages
            ["message_no_data"] = "Sin datos disponibles",
            ["message_try_again"] = "Intentar de nuevo",
            ["message_network_error"] = "Error de red. Por favor verifica tu conexión.",
            ["message_server_error"] = "Error de servidor. Por favor intenta más tarde.",
            ["message_request_timeout"] = "Tiempo de espera agotado. Por favor intenta de nuevo.",
            ["message_unauthorized"] = "No autorizado. Por favor inicia sesión.",
            ["message_forbidden"] = "Acceso denegado.",
            ["message_not_found"] = "Recurso no encontrado.",
            ["message_bad_request"] = "Solicitud inválida. Por favor verifica tu entrada.",
            ["message_conflict"] = "El recurso ya existe.",
            ["message_are_you_sure"] = "¿Estás seguro?",
            ["message_confirmation_required"] = "Confirmación requerida",

            // Dashboard
            ["dashboard_title"] = "Panel de control",
            ["dashboard_current_location"] = "Ubicación actual",
            ["dashboard_no_default_location"] = "Sin ubicación predeterminada establecida. Añada una para empezar.",
            ["dashboard_last_updated"] = "Última actualización: {time}",
            ["dashboard_weather_data_unavailable"] = "Datos climáticos actualmente no disponibles",

            // Validation
            ["validation_required"] = "Este campo es obligatorio",
            ["validation_email_invalid"] = "Dirección de correo inválida",
            ["validation_password_too_short"] = "La contraseña debe tener al menos 8 caracteres",
            ["validation_location_name_required"] = "El nombre de la ubicación es obligatorio",
            ["validation_coordinates_invalid"] = "Coordenadas inválidas",
            ["validation_temperature_invalid"] = "Temperatura fuera del rango válido",
            ["validation_wind_speed_invalid"] = "Velocidad del viento fuera del rango válido",
            ["validation_update_frequency_invalid"] = "La frecuencia de actualización debe estar entre 5 y 1440 minutos",
        };
    }
}

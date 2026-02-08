namespace TruweatherCore.Resources;

/// <summary>
/// Italian (it) language resource strings for Truweather application.
/// </summary>
public static class ItalianResources
{
    public static Dictionary<string, string> GetResources()
    {
        return new Dictionary<string, string>
        {
            // Generale
            ["app_name"] = "Truweather",
            ["app_description"] = "Tracciamento e previsione meteorologici in tempo reale",
            ["ok"] = "OK",
            ["cancel"] = "Annulla",
            ["save"] = "Salva",
            ["delete"] = "Elimina",
            ["edit"] = "Modifica",
            ["close"] = "Chiudi",
            ["loading"] = "Caricamento in corso...",
            ["error"] = "Errore",
            ["success"] = "Successo",
            ["warning"] = "Avviso",

            // Autenticazione
            ["auth_register"] = "Registrati",
            ["auth_login"] = "Accedi",
            ["auth_logout"] = "Esci",
            ["auth_email"] = "Email",
            ["auth_password"] = "Password",
            ["auth_confirm_password"] = "Conferma password",
            ["auth_full_name"] = "Nome completo",
            ["auth_register_title"] = "Crea un account",
            ["auth_login_title"] = "Accedi a Truweather",
            ["auth_register_button"] = "Registrati",
            ["auth_login_button"] = "Accedi",
            ["auth_forgot_password"] = "Password dimenticata?",
            ["auth_already_have_account"] = "Hai già un account? Accedi",
            ["auth_no_account"] = "Non hai un account? Registrati",
            ["auth_registration_successful"] = "Registrazione riuscita! Accedi al tuo account.",
            ["auth_login_successful"] = "Benvenuto di nuovo!",
            ["auth_logout_successful"] = "Logout completato",
            ["auth_invalid_credentials"] = "Email o password non valide",
            ["auth_email_already_registered"] = "L'email è già registrata",
            ["auth_password_mismatch"] = "Le password non corrispondono",
            ["auth_session_expired"] = "La tua sessione è scaduta. Accedi di nuovo.",

            // Meteo
            ["weather_current"] = "Meteo attuale",
            ["weather_forecast"] = "Previsione 7 giorni",
            ["weather_temperature"] = "Temperatura",
            ["weather_feels_like"] = "Temperatura percepita",
            ["weather_condition"] = "Condizione",
            ["weather_humidity"] = "Umidità",
            ["weather_wind_speed"] = "Velocità del vento",
            ["weather_wind_direction"] = "Direzione del vento",
            ["weather_pressure"] = "Pressione atmosferica",
            ["weather_visibility"] = "Visibilità",
            ["weather_uv_index"] = "Indice UV",
            ["weather_precipitation"] = "Precipitazioni",
            ["weather_cloudiness"] = "Copertura nuvolosa",
            ["weather_sunrise"] = "Alba",
            ["weather_sunset"] = "Tramonto",

            // Posizioni
            ["location_saved_locations"] = "Posizioni salvate",
            ["location_add_location"] = "Aggiungi posizione",
            ["location_edit_location"] = "Modifica posizione",
            ["location_delete_location"] = "Elimina posizione",
            ["location_location_name"] = "Nome della posizione",
            ["location_latitude"] = "Latitudine",
            ["location_longitude"] = "Longitudine",
            ["location_set_as_default"] = "Imposta come predefinita",
            ["location_default"] = "Predefinita",
            ["location_no_locations"] = "Nessuna posizione salvata. Aggiungine una per iniziare.",
            ["location_added"] = "Posizione aggiunta con successo",
            ["location_updated"] = "Posizione aggiornata con successo",
            ["location_deleted"] = "Posizione eliminata con successo",
            ["location_invalid_coordinates"] = "Coordinate non valide. La latitudine deve essere tra -90 e 90, la longitudine tra -180 e 180.",
            ["location_confirm_delete"] = "Sei sicuro di voler eliminare questa posizione?",

            // Avvisi
            ["alert_weather_alerts"] = "Avvisi meteo",
            ["alert_create_alert"] = "Crea avviso",
            ["alert_edit_alert"] = "Modifica avviso",
            ["alert_delete_alert"] = "Elimina avviso",
            ["alert_alert_type"] = "Tipo di avviso",
            ["alert_condition"] = "Condizione",
            ["alert_threshold"] = "Soglia",
            ["alert_enabled"] = "Abilitato",
            ["alert_disabled"] = "Disabilitato",
            ["alert_no_alerts"] = "Nessun avviso meteo impostato",
            ["alert_created"] = "Avviso creato con successo",
            ["alert_updated"] = "Avviso aggiornato con successo",
            ["alert_deleted"] = "Avviso eliminato con successo",
            ["alert_confirm_delete"] = "Sei sicuro di voler eliminare questo avviso?",

            // Tipi di avviso
            ["alert_type_temperature"] = "Temperatura",
            ["alert_type_wind_speed"] = "Velocità del vento",
            ["alert_type_humidity"] = "Umidità",
            ["alert_type_pressure"] = "Pressione atmosferica",
            ["alert_type_precipitation"] = "Precipitazioni",

            // Condizioni di avviso
            ["alert_condition_above"] = "Sopra",
            ["alert_condition_below"] = "Sotto",
            ["alert_condition_equals"] = "Uguale a",

            // Preferenze
            ["preferences_user_preferences"] = "Preferenze",
            ["preferences_temperature_unit"] = "Unità di temperatura",
            ["preferences_wind_speed_unit"] = "Unità velocità del vento",
            ["preferences_theme"] = "Tema",
            ["preferences_language"] = "Lingua",
            ["preferences_notifications"] = "Abilita notifiche",
            ["preferences_email_alerts"] = "Abilita avvisi email",
            ["preferences_update_frequency"] = "Frequenza aggiornamento (minuti)",
            ["preferences_saved"] = "Preferenze salvate con successo",

            // Unità
            ["unit_celsius"] = "Celsius (°C)",
            ["unit_fahrenheit"] = "Fahrenheit (°F)",
            ["unit_kelvin"] = "Kelvin (K)",
            ["unit_ms"] = "Metri al secondo (m/s)",
            ["unit_kmh"] = "Chilometri all'ora (km/h)",
            ["unit_mph"] = "Miglia all'ora (mph)",
            ["unit_knots"] = "Nodi (kts)",

            // Temi
            ["theme_light"] = "Chiaro",
            ["theme_dark"] = "Scuro",

            // Lingue
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

            // Navigazione
            ["nav_dashboard"] = "Pannello di controllo",
            ["nav_locations"] = "Posizioni",
            ["nav_alerts"] = "Avvisi",
            ["nav_preferences"] = "Preferenze",
            ["nav_profile"] = "Profilo",

            // Messaggi
            ["message_no_data"] = "Nessun dato disponibile",
            ["message_try_again"] = "Riprova",
            ["message_network_error"] = "Errore di rete. Verifica la tua connessione.",
            ["message_server_error"] = "Errore del server. Riprova più tardi.",
            ["message_request_timeout"] = "Richiesta scaduta. Riprova.",
            ["message_unauthorized"] = "Non autorizzato. Accedi.",
            ["message_forbidden"] = "Accesso negato.",
            ["message_not_found"] = "Risorsa non trovata.",
            ["message_bad_request"] = "Richiesta non valida. Verifica il tuo input.",
            ["message_conflict"] = "La risorsa esiste già.",
            ["message_are_you_sure"] = "Sei sicuro?",
            ["message_confirmation_required"] = "Conferma richiesta",

            // Pannello di controllo
            ["dashboard_title"] = "Pannello di controllo",
            ["dashboard_current_location"] = "Posizione attuale",
            ["dashboard_no_default_location"] = "Nessuna posizione predefinita impostata. Aggiungi una posizione per iniziare.",
            ["dashboard_last_updated"] = "Ultimo aggiornamento: {time}",
            ["dashboard_weather_data_unavailable"] = "Dati meteo non disponibili al momento",

            // Validazione
            ["validation_required"] = "Questo campo è obbligatorio",
            ["validation_email_invalid"] = "Indirizzo email non valido",
            ["validation_password_too_short"] = "La password deve contenere almeno 8 caratteri",
            ["validation_location_name_required"] = "Il nome della posizione è obbligatorio",
            ["validation_coordinates_invalid"] = "Coordinate non valide",
            ["validation_temperature_invalid"] = "Temperatura al di fuori dell'intervallo valido",
            ["validation_wind_speed_invalid"] = "Velocità del vento al di fuori dell'intervallo valido",
            ["validation_update_frequency_invalid"] = "La frequenza di aggiornamento deve essere tra 5 e 1440 minuti",
        };
    }
}

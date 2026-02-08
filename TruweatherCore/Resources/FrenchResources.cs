namespace TruweatherCore.Resources;

/// <summary>
/// French (fr) language resource strings for Truweather application.
/// </summary>
public static class FrenchResources
{
    public static Dictionary<string, string> GetResources()
    {
        return new Dictionary<string, string>
        {
            // General
            ["app_name"] = "Truweather",
            ["app_description"] = "Suivi météorologique en temps réel et prévisions",
            ["ok"] = "OK",
            ["cancel"] = "Annuler",
            ["save"] = "Enregistrer",
            ["delete"] = "Supprimer",
            ["edit"] = "Modifier",
            ["close"] = "Fermer",
            ["loading"] = "Chargement...",
            ["error"] = "Erreur",
            ["success"] = "Succès",
            ["warning"] = "Avertissement",

            // Authentication
            ["auth_register"] = "S'inscrire",
            ["auth_login"] = "Se connecter",
            ["auth_logout"] = "Se déconnecter",
            ["auth_email"] = "Email",
            ["auth_password"] = "Mot de passe",
            ["auth_confirm_password"] = "Confirmer le mot de passe",
            ["auth_full_name"] = "Nom complet",
            ["auth_register_title"] = "Créer un compte",
            ["auth_login_title"] = "Se connecter à Truweather",
            ["auth_register_button"] = "S'inscrire",
            ["auth_login_button"] = "Se connecter",
            ["auth_forgot_password"] = "Mot de passe oublié?",
            ["auth_already_have_account"] = "Vous avez déjà un compte? Se connecter",
            ["auth_no_account"] = "Vous n'avez pas de compte? S'inscrire",
            ["auth_registration_successful"] = "Inscription réussie! Veuillez vous connecter.",
            ["auth_login_successful"] = "Bienvenue!",
            ["auth_logout_successful"] = "Déconnexion réussie",
            ["auth_invalid_credentials"] = "Email ou mot de passe invalide",
            ["auth_email_already_registered"] = "Cet email est déjà enregistré",
            ["auth_password_mismatch"] = "Les mots de passe ne correspondent pas",
            ["auth_session_expired"] = "Votre session a expiré. Veuillez vous reconnecter.",

            // Weather
            ["weather_current"] = "Météo actuelle",
            ["weather_forecast"] = "Prévisions 7 jours",
            ["weather_temperature"] = "Température",
            ["weather_feels_like"] = "Ressenti",
            ["weather_condition"] = "Condition",
            ["weather_humidity"] = "Humidité",
            ["weather_wind_speed"] = "Vitesse du vent",
            ["weather_wind_direction"] = "Direction du vent",
            ["weather_pressure"] = "Pression",
            ["weather_visibility"] = "Visibilité",
            ["weather_uv_index"] = "Indice UV",
            ["weather_precipitation"] = "Précipitation",
            ["weather_cloudiness"] = "Nébulosité",
            ["weather_sunrise"] = "Lever du soleil",
            ["weather_sunset"] = "Coucher du soleil",

            // Locations
            ["location_saved_locations"] = "Lieux enregistrés",
            ["location_add_location"] = "Ajouter un lieu",
            ["location_edit_location"] = "Modifier le lieu",
            ["location_delete_location"] = "Supprimer le lieu",
            ["location_location_name"] = "Nom du lieu",
            ["location_latitude"] = "Latitude",
            ["location_longitude"] = "Longitude",
            ["location_set_as_default"] = "Définir par défaut",
            ["location_default"] = "Par défaut",
            ["location_no_locations"] = "Aucun lieu enregistré. Ajoutez-en un pour commencer.",
            ["location_added"] = "Lieu ajouté avec succès",
            ["location_updated"] = "Lieu mis à jour avec succès",
            ["location_deleted"] = "Lieu supprimé avec succès",
            ["location_invalid_coordinates"] = "Coordonnées invalides. La latitude doit être entre -90 et 90, la longitude entre -180 et 180.",
            ["location_confirm_delete"] = "Êtes-vous sûr de vouloir supprimer ce lieu?",

            // Alerts
            ["alert_weather_alerts"] = "Alertes météorologiques",
            ["alert_create_alert"] = "Créer une alerte",
            ["alert_edit_alert"] = "Modifier l'alerte",
            ["alert_delete_alert"] = "Supprimer l'alerte",
            ["alert_alert_type"] = "Type d'alerte",
            ["alert_condition"] = "Condition",
            ["alert_threshold"] = "Seuil",
            ["alert_enabled"] = "Activée",
            ["alert_disabled"] = "Désactivée",
            ["alert_no_alerts"] = "Aucune alerte météorologique configurée",
            ["alert_created"] = "Alerte créée avec succès",
            ["alert_updated"] = "Alerte mise à jour avec succès",
            ["alert_deleted"] = "Alerte supprimée avec succès",
            ["alert_confirm_delete"] = "Êtes-vous sûr de vouloir supprimer cette alerte?",

            // Alert Types
            ["alert_type_temperature"] = "Température",
            ["alert_type_wind_speed"] = "Vitesse du vent",
            ["alert_type_humidity"] = "Humidité",
            ["alert_type_pressure"] = "Pression",
            ["alert_type_precipitation"] = "Précipitation",

            // Alert Conditions
            ["alert_condition_above"] = "Au-dessus",
            ["alert_condition_below"] = "Au-dessous",
            ["alert_condition_equals"] = "Égal à",

            // Preferences
            ["preferences_user_preferences"] = "Préférences",
            ["preferences_temperature_unit"] = "Unité de température",
            ["preferences_wind_speed_unit"] = "Unité de vitesse du vent",
            ["preferences_theme"] = "Thème",
            ["preferences_language"] = "Langue",
            ["preferences_notifications"] = "Activer les notifications",
            ["preferences_email_alerts"] = "Activer les alertes par email",
            ["preferences_update_frequency"] = "Fréquence de mise à jour (minutes)",
            ["preferences_saved"] = "Préférences enregistrées avec succès",

            // Units
            ["unit_celsius"] = "Celsius (°C)",
            ["unit_fahrenheit"] = "Fahrenheit (°F)",
            ["unit_kelvin"] = "Kelvin (K)",
            ["unit_ms"] = "Mètres par seconde (m/s)",
            ["unit_kmh"] = "Kilomètres par heure (km/h)",
            ["unit_mph"] = "Milles par heure (mph)",
            ["unit_knots"] = "Nœuds (kts)",

            // Themes
            ["theme_light"] = "Clair",
            ["theme_dark"] = "Sombre",

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
            ["nav_dashboard"] = "Tableau de bord",
            ["nav_locations"] = "Lieux",
            ["nav_alerts"] = "Alertes",
            ["nav_preferences"] = "Préférences",
            ["nav_profile"] = "Profil",

            // Messages
            ["message_no_data"] = "Aucune donnée disponible",
            ["message_try_again"] = "Réessayer",
            ["message_network_error"] = "Erreur réseau. Veuillez vérifier votre connexion.",
            ["message_server_error"] = "Erreur serveur. Veuillez réessayer plus tard.",
            ["message_request_timeout"] = "Délai d'attente dépassé. Veuillez réessayer.",
            ["message_unauthorized"] = "Non autorisé. Veuillez vous connecter.",
            ["message_forbidden"] = "Accès refusé.",
            ["message_not_found"] = "Ressource non trouvée.",
            ["message_bad_request"] = "Requête invalide. Veuillez vérifier votre saisie.",
            ["message_conflict"] = "La ressource existe déjà.",
            ["message_are_you_sure"] = "Êtes-vous sûr?",
            ["message_confirmation_required"] = "Confirmation requise",

            // Dashboard
            ["dashboard_title"] = "Tableau de bord",
            ["dashboard_current_location"] = "Lieu actuel",
            ["dashboard_no_default_location"] = "Aucun lieu par défaut défini. Ajoutez-en un pour commencer.",
            ["dashboard_last_updated"] = "Dernière mise à jour: {time}",
            ["dashboard_weather_data_unavailable"] = "Données météorologiques actuellement indisponibles",

            // Validation
            ["validation_required"] = "Ce champ est obligatoire",
            ["validation_email_invalid"] = "Adresse email invalide",
            ["validation_password_too_short"] = "Le mot de passe doit contenir au moins 8 caractères",
            ["validation_location_name_required"] = "Le nom du lieu est obligatoire",
            ["validation_coordinates_invalid"] = "Coordonnées invalides",
            ["validation_temperature_invalid"] = "Température en dehors de la plage valide",
            ["validation_wind_speed_invalid"] = "Vitesse du vent en dehors de la plage valide",
            ["validation_update_frequency_invalid"] = "La fréquence de mise à jour doit être entre 5 et 1440 minutes",
        };
    }
}

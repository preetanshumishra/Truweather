namespace TruweatherCore.Resources;

/// <summary>
/// Portuguese (pt) language resource strings for Truweather application.
/// </summary>
public static class PortugueseResources
{
    public static Dictionary<string, string> GetResources()
    {
        return new Dictionary<string, string>
        {
            // Geral
            ["app_name"] = "Truweather",
            ["app_description"] = "Rastreamento e previsão meteorológica em tempo real",
            ["ok"] = "OK",
            ["cancel"] = "Cancelar",
            ["save"] = "Salvar",
            ["delete"] = "Deletar",
            ["edit"] = "Editar",
            ["close"] = "Fechar",
            ["loading"] = "Carregando...",
            ["error"] = "Erro",
            ["success"] = "Sucesso",
            ["warning"] = "Aviso",

            // Autenticação
            ["auth_register"] = "Registrar",
            ["auth_login"] = "Entrar",
            ["auth_logout"] = "Sair",
            ["auth_email"] = "Email",
            ["auth_password"] = "Senha",
            ["auth_confirm_password"] = "Confirmar senha",
            ["auth_full_name"] = "Nome completo",
            ["auth_register_title"] = "Criar conta",
            ["auth_login_title"] = "Entrar no Truweather",
            ["auth_register_button"] = "Registrar",
            ["auth_login_button"] = "Entrar",
            ["auth_forgot_password"] = "Esqueceu sua senha?",
            ["auth_already_have_account"] = "Já tem uma conta? Entrar",
            ["auth_no_account"] = "Não tem uma conta? Registrar",
            ["auth_registration_successful"] = "Registro bem-sucedido! Por favor faça login.",
            ["auth_login_successful"] = "Bem-vindo de volta!",
            ["auth_logout_successful"] = "Logout bem-sucedido",
            ["auth_invalid_credentials"] = "Email ou senha inválidos",
            ["auth_email_already_registered"] = "Email já está registrado",
            ["auth_password_mismatch"] = "As senhas não correspondem",
            ["auth_session_expired"] = "Sua sessão expirou. Por favor faça login novamente.",

            // Clima
            ["weather_current"] = "Clima atual",
            ["weather_forecast"] = "Previsão de 7 dias",
            ["weather_temperature"] = "Temperatura",
            ["weather_feels_like"] = "Sensação térmica",
            ["weather_condition"] = "Condição",
            ["weather_humidity"] = "Umidade",
            ["weather_wind_speed"] = "Velocidade do vento",
            ["weather_wind_direction"] = "Direção do vento",
            ["weather_pressure"] = "Pressão atmosférica",
            ["weather_visibility"] = "Visibilidade",
            ["weather_uv_index"] = "Índice UV",
            ["weather_precipitation"] = "Precipitação",
            ["weather_cloudiness"] = "Nebulosidade",
            ["weather_sunrise"] = "Nascer do sol",
            ["weather_sunset"] = "Pôr do sol",

            // Localizações
            ["location_saved_locations"] = "Localizações salvas",
            ["location_add_location"] = "Adicionar localização",
            ["location_edit_location"] = "Editar localização",
            ["location_delete_location"] = "Deletar localização",
            ["location_location_name"] = "Nome da localização",
            ["location_latitude"] = "Latitude",
            ["location_longitude"] = "Longitude",
            ["location_set_as_default"] = "Definir como padrão",
            ["location_default"] = "Padrão",
            ["location_no_locations"] = "Nenhuma localização salva. Adicione uma para começar.",
            ["location_added"] = "Localização adicionada com sucesso",
            ["location_updated"] = "Localização atualizada com sucesso",
            ["location_deleted"] = "Localização deletada com sucesso",
            ["location_invalid_coordinates"] = "Coordenadas inválidas. A latitude deve estar entre -90 e 90, a longitude entre -180 e 180.",
            ["location_confirm_delete"] = "Você tem certeza que deseja deletar esta localização?",

            // Alertas
            ["alert_weather_alerts"] = "Alertas meteorológicos",
            ["alert_create_alert"] = "Criar alerta",
            ["alert_edit_alert"] = "Editar alerta",
            ["alert_delete_alert"] = "Deletar alerta",
            ["alert_alert_type"] = "Tipo de alerta",
            ["alert_condition"] = "Condição",
            ["alert_threshold"] = "Limite",
            ["alert_enabled"] = "Ativado",
            ["alert_disabled"] = "Desativado",
            ["alert_no_alerts"] = "Nenhum alerta meteorológico configurado",
            ["alert_created"] = "Alerta criado com sucesso",
            ["alert_updated"] = "Alerta atualizado com sucesso",
            ["alert_deleted"] = "Alerta deletado com sucesso",
            ["alert_confirm_delete"] = "Você tem certeza que deseja deletar este alerta?",

            // Tipos de alerta
            ["alert_type_temperature"] = "Temperatura",
            ["alert_type_wind_speed"] = "Velocidade do vento",
            ["alert_type_humidity"] = "Umidade",
            ["alert_type_pressure"] = "Pressão atmosférica",
            ["alert_type_precipitation"] = "Precipitação",

            // Condições de alerta
            ["alert_condition_above"] = "Acima",
            ["alert_condition_below"] = "Abaixo",
            ["alert_condition_equals"] = "Igual a",

            // Preferências
            ["preferences_user_preferences"] = "Preferências",
            ["preferences_temperature_unit"] = "Unidade de temperatura",
            ["preferences_wind_speed_unit"] = "Unidade de velocidade do vento",
            ["preferences_theme"] = "Tema",
            ["preferences_language"] = "Idioma",
            ["preferences_notifications"] = "Ativar notificações",
            ["preferences_email_alerts"] = "Ativar alertas por email",
            ["preferences_update_frequency"] = "Frequência de atualização (minutos)",
            ["preferences_saved"] = "Preferências salvas com sucesso",

            // Unidades
            ["unit_celsius"] = "Celsius (°C)",
            ["unit_fahrenheit"] = "Fahrenheit (°F)",
            ["unit_kelvin"] = "Kelvin (K)",
            ["unit_ms"] = "Metros por segundo (m/s)",
            ["unit_kmh"] = "Quilômetros por hora (km/h)",
            ["unit_mph"] = "Milhas por hora (mph)",
            ["unit_knots"] = "Nós (kts)",

            // Temas
            ["theme_light"] = "Claro",
            ["theme_dark"] = "Escuro",

            // Idiomas
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

            // Navegação
            ["nav_dashboard"] = "Painel",
            ["nav_locations"] = "Localizações",
            ["nav_alerts"] = "Alertas",
            ["nav_preferences"] = "Preferências",
            ["nav_profile"] = "Perfil",

            // Mensagens
            ["message_no_data"] = "Nenhum dado disponível",
            ["message_try_again"] = "Tentar novamente",
            ["message_network_error"] = "Erro de rede. Verifique sua conexão.",
            ["message_server_error"] = "Erro do servidor. Tente novamente mais tarde.",
            ["message_request_timeout"] = "Solicitação expirou. Tente novamente.",
            ["message_unauthorized"] = "Não autorizado. Por favor faça login.",
            ["message_forbidden"] = "Acesso negado.",
            ["message_not_found"] = "Recurso não encontrado.",
            ["message_bad_request"] = "Solicitação inválida. Verifique sua entrada.",
            ["message_conflict"] = "O recurso já existe.",
            ["message_are_you_sure"] = "Você tem certeza?",
            ["message_confirmation_required"] = "Confirmação necessária",

            // Painel
            ["dashboard_title"] = "Painel",
            ["dashboard_current_location"] = "Localização atual",
            ["dashboard_no_default_location"] = "Nenhuma localização padrão definida. Adicione uma para começar.",
            ["dashboard_last_updated"] = "Última atualização: {time}",
            ["dashboard_weather_data_unavailable"] = "Dados meteorológicos indisponíveis no momento",

            // Validação
            ["validation_required"] = "Este campo é obrigatório",
            ["validation_email_invalid"] = "Endereço de email inválido",
            ["validation_password_too_short"] = "A senha deve ter pelo menos 8 caracteres",
            ["validation_location_name_required"] = "O nome da localização é obrigatório",
            ["validation_coordinates_invalid"] = "Coordenadas inválidas",
            ["validation_temperature_invalid"] = "Temperatura fora do intervalo válido",
            ["validation_wind_speed_invalid"] = "Velocidade do vento fora do intervalo válido",
            ["validation_update_frequency_invalid"] = "A frequência de atualização deve estar entre 5 e 1440 minutos",
        };
    }
}

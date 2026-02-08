namespace TruweatherCore.Resources;

/// <summary>
/// Russian (ru) language resource strings for Truweather application.
/// </summary>
public static class RussianResources
{
    public static Dictionary<string, string> GetResources()
    {
        return new Dictionary<string, string>
        {
            // Общее
            ["app_name"] = "Truweather",
            ["app_description"] = "Отслеживание и прогноз погоды в реальном времени",
            ["ok"] = "OK",
            ["cancel"] = "Отмена",
            ["save"] = "Сохранить",
            ["delete"] = "Удалить",
            ["edit"] = "Редактировать",
            ["close"] = "Закрыть",
            ["loading"] = "Загрузка...",
            ["error"] = "Ошибка",
            ["success"] = "Успешно",
            ["warning"] = "Предупреждение",

            // Аутентификация
            ["auth_register"] = "Зарегистрироваться",
            ["auth_login"] = "Войти",
            ["auth_logout"] = "Выход",
            ["auth_email"] = "Электронная почта",
            ["auth_password"] = "Пароль",
            ["auth_confirm_password"] = "Подтвердите пароль",
            ["auth_full_name"] = "Полное имя",
            ["auth_register_title"] = "Создать учетную запись",
            ["auth_login_title"] = "Войти в Truweather",
            ["auth_register_button"] = "Зарегистрироваться",
            ["auth_login_button"] = "Войти",
            ["auth_forgot_password"] = "Забыли пароль?",
            ["auth_already_have_account"] = "Уже есть учетная запись? Войти",
            ["auth_no_account"] = "Нет учетной записи? Зарегистрироваться",
            ["auth_registration_successful"] = "Регистрация успешна! Пожалуйста войдите.",
            ["auth_login_successful"] = "Добро пожаловать обратно!",
            ["auth_logout_successful"] = "Вы успешно вышли",
            ["auth_invalid_credentials"] = "Неверная электронная почта или пароль",
            ["auth_email_already_registered"] = "Электронная почта уже зарегистрирована",
            ["auth_password_mismatch"] = "Пароли не совпадают",
            ["auth_session_expired"] = "Ваша сессия истекла. Пожалуйста войдите снова.",

            // Погода
            ["weather_current"] = "Текущая погода",
            ["weather_forecast"] = "Прогноз на 7 дней",
            ["weather_temperature"] = "Температура",
            ["weather_feels_like"] = "Ощущается как",
            ["weather_condition"] = "Условие",
            ["weather_humidity"] = "Влажность",
            ["weather_wind_speed"] = "Скорость ветра",
            ["weather_wind_direction"] = "Направление ветра",
            ["weather_pressure"] = "Атмосферное давление",
            ["weather_visibility"] = "Видимость",
            ["weather_uv_index"] = "Индекс УФ",
            ["weather_precipitation"] = "Осадки",
            ["weather_cloudiness"] = "Облачность",
            ["weather_sunrise"] = "Восход солнца",
            ["weather_sunset"] = "Закат солнца",

            // Места
            ["location_saved_locations"] = "Сохраненные места",
            ["location_add_location"] = "Добавить место",
            ["location_edit_location"] = "Редактировать место",
            ["location_delete_location"] = "Удалить место",
            ["location_location_name"] = "Название места",
            ["location_latitude"] = "Широта",
            ["location_longitude"] = "Долгота",
            ["location_set_as_default"] = "Установить по умолчанию",
            ["location_default"] = "По умолчанию",
            ["location_no_locations"] = "Нет сохраненных мест. Добавьте одно для начала.",
            ["location_added"] = "Место успешно добавлено",
            ["location_updated"] = "Место успешно обновлено",
            ["location_deleted"] = "Место успешно удалено",
            ["location_invalid_coordinates"] = "Неверные координаты. Широта должна быть от -90 до 90, долгота от -180 до 180.",
            ["location_confirm_delete"] = "Вы уверены, что хотите удалить это место?",

            // Оповещения
            ["alert_weather_alerts"] = "Погодные оповещения",
            ["alert_create_alert"] = "Создать оповещение",
            ["alert_edit_alert"] = "Редактировать оповещение",
            ["alert_delete_alert"] = "Удалить оповещение",
            ["alert_alert_type"] = "Тип оповещения",
            ["alert_condition"] = "Условие",
            ["alert_threshold"] = "Пороговое значение",
            ["alert_enabled"] = "Включено",
            ["alert_disabled"] = "Отключено",
            ["alert_no_alerts"] = "Погодные оповещения не установлены",
            ["alert_created"] = "Оповещение успешно создано",
            ["alert_updated"] = "Оповещение успешно обновлено",
            ["alert_deleted"] = "Оповещение успешно удалено",
            ["alert_confirm_delete"] = "Вы уверены, что хотите удалить это оповещение?",

            // Типы оповещений
            ["alert_type_temperature"] = "Температура",
            ["alert_type_wind_speed"] = "Скорость ветра",
            ["alert_type_humidity"] = "Влажность",
            ["alert_type_pressure"] = "Атмосферное давление",
            ["alert_type_precipitation"] = "Осадки",

            // Условия оповещений
            ["alert_condition_above"] = "Выше",
            ["alert_condition_below"] = "Ниже",
            ["alert_condition_equals"] = "Равно",

            // Предпочтения
            ["preferences_user_preferences"] = "Предпочтения",
            ["preferences_temperature_unit"] = "Единица температуры",
            ["preferences_wind_speed_unit"] = "Единица скорости ветра",
            ["preferences_theme"] = "Тема",
            ["preferences_language"] = "Язык",
            ["preferences_notifications"] = "Включить уведомления",
            ["preferences_email_alerts"] = "Включить уведомления по электронной почте",
            ["preferences_update_frequency"] = "Частота обновления (минуты)",
            ["preferences_saved"] = "Предпочтения успешно сохранены",

            // Единицы
            ["unit_celsius"] = "Цельсий (°C)",
            ["unit_fahrenheit"] = "Фаренгейт (°F)",
            ["unit_kelvin"] = "Кельвин (K)",
            ["unit_ms"] = "Метры в секунду (м/с)",
            ["unit_kmh"] = "Километры в час (км/ч)",
            ["unit_mph"] = "Мили в час (mph)",
            ["unit_knots"] = "Узлы (кт)",

            // Темы
            ["theme_light"] = "Светлая",
            ["theme_dark"] = "Темная",

            // Языки
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

            // Навигация
            ["nav_dashboard"] = "Панель управления",
            ["nav_locations"] = "Места",
            ["nav_alerts"] = "Оповещения",
            ["nav_preferences"] = "Предпочтения",
            ["nav_profile"] = "Профиль",

            // Сообщения
            ["message_no_data"] = "Данные недоступны",
            ["message_try_again"] = "Повторить попытку",
            ["message_network_error"] = "Ошибка сети. Проверьте ваше соединение.",
            ["message_server_error"] = "Ошибка сервера. Повторите попытку позже.",
            ["message_request_timeout"] = "Истекло время запроса. Повторите попытку.",
            ["message_unauthorized"] = "Не авторизировано. Пожалуйста войдите.",
            ["message_forbidden"] = "Доступ запрещен.",
            ["message_not_found"] = "Ресурс не найден.",
            ["message_bad_request"] = "Неверный запрос. Проверьте ваш ввод.",
            ["message_conflict"] = "Ресурс уже существует.",
            ["message_are_you_sure"] = "Вы уверены?",
            ["message_confirmation_required"] = "Требуется подтверждение",

            // Панель управления
            ["dashboard_title"] = "Панель управления",
            ["dashboard_current_location"] = "Текущее место",
            ["dashboard_no_default_location"] = "Нет установленного места по умолчанию. Добавьте одно для начала.",
            ["dashboard_last_updated"] = "Последнее обновление: {time}",
            ["dashboard_weather_data_unavailable"] = "Данные о погоде в настоящее время недоступны",

            // Валидация
            ["validation_required"] = "Это поле обязательно",
            ["validation_email_invalid"] = "Неверный адрес электронной почты",
            ["validation_password_too_short"] = "Пароль должен содержать не менее 8 символов",
            ["validation_location_name_required"] = "Требуется название места",
            ["validation_coordinates_invalid"] = "Неверные координаты",
            ["validation_temperature_invalid"] = "Температура вне допустимого диапазона",
            ["validation_wind_speed_invalid"] = "Скорость ветра вне допустимого диапазона",
            ["validation_update_frequency_invalid"] = "Частота обновления должна быть от 5 до 1440 минут",
        };
    }
}

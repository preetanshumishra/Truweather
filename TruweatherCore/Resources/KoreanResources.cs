namespace TruweatherCore.Resources;

/// <summary>
/// Korean (ko) language resource strings for Truweather application.
/// </summary>
public static class KoreanResources
{
    public static Dictionary<string, string> GetResources()
    {
        return new Dictionary<string, string>
        {
            // 일반
            ["app_name"] = "Truweather",
            ["app_description"] = "실시간 날씨 추적 및 예보",
            ["ok"] = "확인",
            ["cancel"] = "취소",
            ["save"] = "저장",
            ["delete"] = "삭제",
            ["edit"] = "편집",
            ["close"] = "닫기",
            ["loading"] = "로드 중...",
            ["error"] = "오류",
            ["success"] = "성공",
            ["warning"] = "경고",

            // 인증
            ["auth_register"] = "가입",
            ["auth_login"] = "로그인",
            ["auth_logout"] = "로그아웃",
            ["auth_email"] = "이메일",
            ["auth_password"] = "비밀번호",
            ["auth_confirm_password"] = "비밀번호 확인",
            ["auth_full_name"] = "전체 이름",
            ["auth_register_title"] = "계정 만들기",
            ["auth_login_title"] = "Truweather에 로그인",
            ["auth_register_button"] = "가입",
            ["auth_login_button"] = "로그인",
            ["auth_forgot_password"] = "비밀번호를 잊으셨나요?",
            ["auth_already_have_account"] = "이미 계정이 있으신가요? 로그인",
            ["auth_no_account"] = "계정이 없으신가요? 가입",
            ["auth_registration_successful"] = "가입이 성공했습니다! 로그인해주세요.",
            ["auth_login_successful"] = "다시 오셨군요!",
            ["auth_logout_successful"] = "로그아웃되었습니다",
            ["auth_invalid_credentials"] = "잘못된 이메일 또는 비밀번호",
            ["auth_email_already_registered"] = "이메일이 이미 등록되었습니다",
            ["auth_password_mismatch"] = "비밀번호가 일치하지 않습니다",
            ["auth_session_expired"] = "세션이 만료되었습니다. 다시 로그인해주세요.",

            // 날씨
            ["weather_current"] = "현재 날씨",
            ["weather_forecast"] = "7일 예보",
            ["weather_temperature"] = "온도",
            ["weather_feels_like"] = "체감 온도",
            ["weather_condition"] = "날씨",
            ["weather_humidity"] = "습도",
            ["weather_wind_speed"] = "풍속",
            ["weather_wind_direction"] = "풍향",
            ["weather_pressure"] = "기압",
            ["weather_visibility"] = "시정",
            ["weather_uv_index"] = "자외선지수",
            ["weather_precipitation"] = "강수량",
            ["weather_cloudiness"] = "구름",
            ["weather_sunrise"] = "일출",
            ["weather_sunset"] = "일몰",

            // 위치
            ["location_saved_locations"] = "저장된 위치",
            ["location_add_location"] = "위치 추가",
            ["location_edit_location"] = "위치 편집",
            ["location_delete_location"] = "위치 삭제",
            ["location_location_name"] = "위치 이름",
            ["location_latitude"] = "위도",
            ["location_longitude"] = "경도",
            ["location_set_as_default"] = "기본값으로 설정",
            ["location_default"] = "기본",
            ["location_no_locations"] = "저장된 위치가 없습니다. 시작하려면 하나를 추가하세요.",
            ["location_added"] = "위치가 성공적으로 추가되었습니다",
            ["location_updated"] = "위치가 성공적으로 업데이트되었습니다",
            ["location_deleted"] = "위치가 성공적으로 삭제되었습니다",
            ["location_invalid_coordinates"] = "잘못된 좌표입니다. 위도는 -90에서 90, 경도는 -180에서 180 사이여야 합니다.",
            ["location_confirm_delete"] = "이 위치를 정말 삭제하시겠습니까?",

            // 경보
            ["alert_weather_alerts"] = "날씨 경보",
            ["alert_create_alert"] = "경보 생성",
            ["alert_edit_alert"] = "경보 편집",
            ["alert_delete_alert"] = "경보 삭제",
            ["alert_alert_type"] = "경보 유형",
            ["alert_condition"] = "조건",
            ["alert_threshold"] = "임계값",
            ["alert_enabled"] = "활성화됨",
            ["alert_disabled"] = "비활성화됨",
            ["alert_no_alerts"] = "설정된 날씨 경보가 없습니다",
            ["alert_created"] = "경보가 성공적으로 생성되었습니다",
            ["alert_updated"] = "경보가 성공적으로 업데이트되었습니다",
            ["alert_deleted"] = "경보가 성공적으로 삭제되었습니다",
            ["alert_confirm_delete"] = "이 경보를 정말 삭제하시겠습니까?",

            // 경보 유형
            ["alert_type_temperature"] = "온도",
            ["alert_type_wind_speed"] = "풍속",
            ["alert_type_humidity"] = "습도",
            ["alert_type_pressure"] = "기압",
            ["alert_type_precipitation"] = "강수량",

            // 경보 조건
            ["alert_condition_above"] = "위",
            ["alert_condition_below"] = "아래",
            ["alert_condition_equals"] = "같음",

            // 기본 설정
            ["preferences_user_preferences"] = "기본 설정",
            ["preferences_temperature_unit"] = "온도 단위",
            ["preferences_wind_speed_unit"] = "풍속 단위",
            ["preferences_theme"] = "테마",
            ["preferences_language"] = "언어",
            ["preferences_notifications"] = "알림 활성화",
            ["preferences_email_alerts"] = "이메일 경보 활성화",
            ["preferences_update_frequency"] = "업데이트 빈도(분)",
            ["preferences_saved"] = "기본 설정이 성공적으로 저장되었습니다",

            // 단위
            ["unit_celsius"] = "섭씨 (°C)",
            ["unit_fahrenheit"] = "화씨 (°F)",
            ["unit_kelvin"] = "켈빈 (K)",
            ["unit_ms"] = "초당 미터 (m/s)",
            ["unit_kmh"] = "시간당 킬로미터 (km/h)",
            ["unit_mph"] = "시간당 마일 (mph)",
            ["unit_knots"] = "노트 (kts)",

            // 테마
            ["theme_light"] = "밝음",
            ["theme_dark"] = "어두움",

            // 언어
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

            // 네비게이션
            ["nav_dashboard"] = "대시보드",
            ["nav_locations"] = "위치",
            ["nav_alerts"] = "경보",
            ["nav_preferences"] = "기본 설정",
            ["nav_profile"] = "프로필",

            // 메시지
            ["message_no_data"] = "사용 가능한 데이터 없음",
            ["message_try_again"] = "다시 시도",
            ["message_network_error"] = "네트워크 오류입니다. 연결을 확인하세요.",
            ["message_server_error"] = "서버 오류입니다. 나중에 다시 시도하세요.",
            ["message_request_timeout"] = "요청이 시간 초과되었습니다. 다시 시도하세요.",
            ["message_unauthorized"] = "승인되지 않았습니다. 로그인하세요.",
            ["message_forbidden"] = "접근이 거부되었습니다.",
            ["message_not_found"] = "리소스를 찾을 수 없습니다.",
            ["message_bad_request"] = "잘못된 요청입니다. 입력을 확인하세요.",
            ["message_conflict"] = "리소스가 이미 존재합니다.",
            ["message_are_you_sure"] = "확실하신가요?",
            ["message_confirmation_required"] = "확인이 필요합니다",

            // 대시보드
            ["dashboard_title"] = "대시보드",
            ["dashboard_current_location"] = "현재 위치",
            ["dashboard_no_default_location"] = "기본 위치가 설정되지 않았습니다. 시작하려면 하나를 추가하세요.",
            ["dashboard_last_updated"] = "마지막 업데이트: {time}",
            ["dashboard_weather_data_unavailable"] = "날씨 데이터를 현재 사용할 수 없습니다",

            // 검증
            ["validation_required"] = "이 필드는 필수입니다",
            ["validation_email_invalid"] = "유효하지 않은 이메일 주소",
            ["validation_password_too_short"] = "비밀번호는 최소 8자 이상이어야 합니다",
            ["validation_location_name_required"] = "위치 이름이 필요합니다",
            ["validation_coordinates_invalid"] = "유효하지 않은 좌표",
            ["validation_temperature_invalid"] = "온도가 유효한 범위를 벗어났습니다",
            ["validation_wind_speed_invalid"] = "풍속이 유효한 범위를 벗어났습니다",
            ["validation_update_frequency_invalid"] = "업데이트 빈도는 5분에서 1440분 사이여야 합니다",
        };
    }
}

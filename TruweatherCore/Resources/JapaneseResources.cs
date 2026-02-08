namespace TruweatherCore.Resources;

/// <summary>
/// Japanese (ja) language resource strings for Truweather application.
/// </summary>
public static class JapaneseResources
{
    public static Dictionary<string, string> GetResources()
    {
        return new Dictionary<string, string>
        {
            // 一般
            ["app_name"] = "Truweather",
            ["app_description"] = "リアルタイム気象追跡と予報",
            ["ok"] = "OK",
            ["cancel"] = "キャンセル",
            ["save"] = "保存",
            ["delete"] = "削除",
            ["edit"] = "編集",
            ["close"] = "閉じる",
            ["loading"] = "読み込み中...",
            ["error"] = "エラー",
            ["success"] = "成功",
            ["warning"] = "警告",

            // 認証
            ["auth_register"] = "登録",
            ["auth_login"] = "ログイン",
            ["auth_logout"] = "ログアウト",
            ["auth_email"] = "メールアドレス",
            ["auth_password"] = "パスワード",
            ["auth_confirm_password"] = "パスワード確認",
            ["auth_full_name"] = "フルネーム",
            ["auth_register_title"] = "アカウント作成",
            ["auth_login_title"] = "Truweatherにログイン",
            ["auth_register_button"] = "登録",
            ["auth_login_button"] = "ログイン",
            ["auth_forgot_password"] = "パスワードを忘れましたか?",
            ["auth_already_have_account"] = "アカウントをお持ちですか? ログイン",
            ["auth_no_account"] = "アカウントをお持ちではありませんか? 登録",
            ["auth_registration_successful"] = "登録に成功しました!ログインしてください。",
            ["auth_login_successful"] = "おかえりなさい!",
            ["auth_logout_successful"] = "ログアウトしました",
            ["auth_invalid_credentials"] = "無効なメールアドレスまたはパスワード",
            ["auth_email_already_registered"] = "メールアドレスは既に登録されています",
            ["auth_password_mismatch"] = "パスワードが一致しません",
            ["auth_session_expired"] = "セッションの有効期限が切れました。もう一度ログインしてください。",

            // 天気
            ["weather_current"] = "現在の天気",
            ["weather_forecast"] = "7日間予報",
            ["weather_temperature"] = "気温",
            ["weather_feels_like"] = "体感温度",
            ["weather_condition"] = "天気状態",
            ["weather_humidity"] = "湿度",
            ["weather_wind_speed"] = "風速",
            ["weather_wind_direction"] = "風向",
            ["weather_pressure"] = "気圧",
            ["weather_visibility"] = "視程",
            ["weather_uv_index"] = "紫外線指数",
            ["weather_precipitation"] = "降水",
            ["weather_cloudiness"] = "雲量",
            ["weather_sunrise"] = "日の出",
            ["weather_sunset"] = "日の入り",

            // 場所
            ["location_saved_locations"] = "保存済みの場所",
            ["location_add_location"] = "場所を追加",
            ["location_edit_location"] = "場所を編集",
            ["location_delete_location"] = "場所を削除",
            ["location_location_name"] = "場所の名前",
            ["location_latitude"] = "緯度",
            ["location_longitude"] = "経度",
            ["location_set_as_default"] = "デフォルトとして設定",
            ["location_default"] = "デフォルト",
            ["location_no_locations"] = "保存済みの場所がありません。始めるために1つ追加してください。",
            ["location_added"] = "場所が正常に追加されました",
            ["location_updated"] = "場所が正常に更新されました",
            ["location_deleted"] = "場所が正常に削除されました",
            ["location_invalid_coordinates"] = "無効な座標。緯度は-90から90、経度は-180から180である必要があります。",
            ["location_confirm_delete"] = "この場所を本当に削除しますか?",

            // アラート
            ["alert_weather_alerts"] = "気象警報",
            ["alert_create_alert"] = "アラートを作成",
            ["alert_edit_alert"] = "アラートを編集",
            ["alert_delete_alert"] = "アラートを削除",
            ["alert_alert_type"] = "アラートタイプ",
            ["alert_condition"] = "条件",
            ["alert_threshold"] = "しきい値",
            ["alert_enabled"] = "有効",
            ["alert_disabled"] = "無効",
            ["alert_no_alerts"] = "気象警報が設定されていません",
            ["alert_created"] = "アラートが正常に作成されました",
            ["alert_updated"] = "アラートが正常に更新されました",
            ["alert_deleted"] = "アラートが正常に削除されました",
            ["alert_confirm_delete"] = "このアラートを本当に削除しますか?",

            // アラートタイプ
            ["alert_type_temperature"] = "気温",
            ["alert_type_wind_speed"] = "風速",
            ["alert_type_humidity"] = "湿度",
            ["alert_type_pressure"] = "気圧",
            ["alert_type_precipitation"] = "降水",

            // アラート条件
            ["alert_condition_above"] = "より高い",
            ["alert_condition_below"] = "より低い",
            ["alert_condition_equals"] = "等しい",

            // 環境設定
            ["preferences_user_preferences"] = "設定",
            ["preferences_temperature_unit"] = "気温の単位",
            ["preferences_wind_speed_unit"] = "風速の単位",
            ["preferences_theme"] = "テーマ",
            ["preferences_language"] = "言語",
            ["preferences_notifications"] = "通知を有効にする",
            ["preferences_email_alerts"] = "メールアラートを有効にする",
            ["preferences_update_frequency"] = "更新頻度(分)",
            ["preferences_saved"] = "設定が正常に保存されました",

            // ユニット
            ["unit_celsius"] = "セルシウス (°C)",
            ["unit_fahrenheit"] = "華氏 (°F)",
            ["unit_kelvin"] = "ケルビン (K)",
            ["unit_ms"] = "毎秒メートル (m/s)",
            ["unit_kmh"] = "時速キロメートル (km/h)",
            ["unit_mph"] = "時速マイル (mph)",
            ["unit_knots"] = "ノット (kts)",

            // テーマ
            ["theme_light"] = "ライト",
            ["theme_dark"] = "ダーク",

            // 言語
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

            // ナビゲーション
            ["nav_dashboard"] = "ダッシュボード",
            ["nav_locations"] = "場所",
            ["nav_alerts"] = "アラート",
            ["nav_preferences"] = "設定",
            ["nav_profile"] = "プロフィール",

            // メッセージ
            ["message_no_data"] = "利用可能なデータがありません",
            ["message_try_again"] = "もう一度試してください",
            ["message_network_error"] = "ネットワークエラー。接続を確認してください。",
            ["message_server_error"] = "サーバーエラー。後でもう一度お試しください。",
            ["message_request_timeout"] = "リクエストがタイムアウトしました。もう一度お試しください。",
            ["message_unauthorized"] = "認可されていません。ログインしてください。",
            ["message_forbidden"] = "アクセスが拒否されました。",
            ["message_not_found"] = "リソースが見つかりません。",
            ["message_bad_request"] = "無効なリクエスト。入力を確認してください。",
            ["message_conflict"] = "リソースは既に存在します。",
            ["message_are_you_sure"] = "よろしいですか?",
            ["message_confirmation_required"] = "確認が必要です",

            // ダッシュボード
            ["dashboard_title"] = "ダッシュボード",
            ["dashboard_current_location"] = "現在の場所",
            ["dashboard_no_default_location"] = "デフォルトの場所が設定されていません。始めるために1つ追加してください。",
            ["dashboard_last_updated"] = "最終更新: {time}",
            ["dashboard_weather_data_unavailable"] = "天気データは現在利用できません",

            // 検証
            ["validation_required"] = "このフィールドは必須です",
            ["validation_email_invalid"] = "無効なメールアドレス",
            ["validation_password_too_short"] = "パスワードは最低8文字である必要があります",
            ["validation_location_name_required"] = "場所の名前が必要です",
            ["validation_coordinates_invalid"] = "無効な座標",
            ["validation_temperature_invalid"] = "気温が有効な範囲外です",
            ["validation_wind_speed_invalid"] = "風速が有効な範囲外です",
            ["validation_update_frequency_invalid"] = "更新頻度は5分から1440分の間である必要があります",
        };
    }
}

namespace TruweatherCore.Resources;

/// <summary>
/// Chinese (zh) language resource strings for Truweather application.
/// </summary>
public static class ChineseResources
{
    public static Dictionary<string, string> GetResources()
    {
        return new Dictionary<string, string>
        {
            // 常用
            ["app_name"] = "Truweather",
            ["app_description"] = "实时天气追踪和预测",
            ["ok"] = "确定",
            ["cancel"] = "取消",
            ["save"] = "保存",
            ["delete"] = "删除",
            ["edit"] = "编辑",
            ["close"] = "关闭",
            ["loading"] = "加载中...",
            ["error"] = "错误",
            ["success"] = "成功",
            ["warning"] = "警告",

            // 身份验证
            ["auth_register"] = "注册",
            ["auth_login"] = "登录",
            ["auth_logout"] = "登出",
            ["auth_email"] = "电子邮件",
            ["auth_password"] = "密码",
            ["auth_confirm_password"] = "确认密码",
            ["auth_full_name"] = "全名",
            ["auth_register_title"] = "创建账户",
            ["auth_login_title"] = "登录 Truweather",
            ["auth_register_button"] = "注册",
            ["auth_login_button"] = "登录",
            ["auth_forgot_password"] = "忘记密码?",
            ["auth_already_have_account"] = "已有账户? 登录",
            ["auth_no_account"] = "没有账户? 注册",
            ["auth_registration_successful"] = "注册成功!请登录。",
            ["auth_login_successful"] = "欢迎回来!",
            ["auth_logout_successful"] = "登出成功",
            ["auth_invalid_credentials"] = "无效的电子邮件或密码",
            ["auth_email_already_registered"] = "电子邮件已注册",
            ["auth_password_mismatch"] = "密码不匹配",
            ["auth_session_expired"] = "您的会话已过期。请重新登录。",

            // 天气
            ["weather_current"] = "当前天气",
            ["weather_forecast"] = "7天预报",
            ["weather_temperature"] = "温度",
            ["weather_feels_like"] = "体感温度",
            ["weather_condition"] = "天气状况",
            ["weather_humidity"] = "湿度",
            ["weather_wind_speed"] = "风速",
            ["weather_wind_direction"] = "风向",
            ["weather_pressure"] = "气压",
            ["weather_visibility"] = "能见度",
            ["weather_uv_index"] = "紫外线指数",
            ["weather_precipitation"] = "降水",
            ["weather_cloudiness"] = "云量",
            ["weather_sunrise"] = "日出",
            ["weather_sunset"] = "日落",

            // 地点
            ["location_saved_locations"] = "已保存的地点",
            ["location_add_location"] = "添加地点",
            ["location_edit_location"] = "编辑地点",
            ["location_delete_location"] = "删除地点",
            ["location_location_name"] = "地点名称",
            ["location_latitude"] = "纬度",
            ["location_longitude"] = "经度",
            ["location_set_as_default"] = "设为默认",
            ["location_default"] = "默认",
            ["location_no_locations"] = "没有已保存的地点。添加一个以开始。",
            ["location_added"] = "地点已成功添加",
            ["location_updated"] = "地点已成功更新",
            ["location_deleted"] = "地点已成功删除",
            ["location_invalid_coordinates"] = "无效的坐标。纬度必须在 -90 到 90 之间,经度在 -180 到 180 之间。",
            ["location_confirm_delete"] = "您确定要删除此地点吗?",

            // 警报
            ["alert_weather_alerts"] = "天气警报",
            ["alert_create_alert"] = "创建警报",
            ["alert_edit_alert"] = "编辑警报",
            ["alert_delete_alert"] = "删除警报",
            ["alert_alert_type"] = "警报类型",
            ["alert_condition"] = "条件",
            ["alert_threshold"] = "阈值",
            ["alert_enabled"] = "已启用",
            ["alert_disabled"] = "已禁用",
            ["alert_no_alerts"] = "未设置天气警报",
            ["alert_created"] = "警报已成功创建",
            ["alert_updated"] = "警报已成功更新",
            ["alert_deleted"] = "警报已成功删除",
            ["alert_confirm_delete"] = "您确定要删除此警报吗?",

            // 警报类型
            ["alert_type_temperature"] = "温度",
            ["alert_type_wind_speed"] = "风速",
            ["alert_type_humidity"] = "湿度",
            ["alert_type_pressure"] = "气压",
            ["alert_type_precipitation"] = "降水",

            // 警报条件
            ["alert_condition_above"] = "上方",
            ["alert_condition_below"] = "下方",
            ["alert_condition_equals"] = "等于",

            // 偏好设置
            ["preferences_user_preferences"] = "偏好设置",
            ["preferences_temperature_unit"] = "温度单位",
            ["preferences_wind_speed_unit"] = "风速单位",
            ["preferences_theme"] = "主题",
            ["preferences_language"] = "语言",
            ["preferences_notifications"] = "启用通知",
            ["preferences_email_alerts"] = "启用电子邮件警报",
            ["preferences_update_frequency"] = "更新频率(分钟)",
            ["preferences_saved"] = "偏好设置已成功保存",

            // 单位
            ["unit_celsius"] = "摄氏度 (°C)",
            ["unit_fahrenheit"] = "华氏度 (°F)",
            ["unit_kelvin"] = "开尔文 (K)",
            ["unit_ms"] = "米/秒 (m/s)",
            ["unit_kmh"] = "公里/小时 (km/h)",
            ["unit_mph"] = "英里/小时 (mph)",
            ["unit_knots"] = "节 (kts)",

            // 主题
            ["theme_light"] = "浅色",
            ["theme_dark"] = "深色",

            // 语言
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

            // 导航
            ["nav_dashboard"] = "仪表板",
            ["nav_locations"] = "地点",
            ["nav_alerts"] = "警报",
            ["nav_preferences"] = "偏好设置",
            ["nav_profile"] = "个人资料",

            // 消息
            ["message_no_data"] = "没有可用数据",
            ["message_try_again"] = "重试",
            ["message_network_error"] = "网络错误。请检查您的连接。",
            ["message_server_error"] = "服务器错误。请稍后再试。",
            ["message_request_timeout"] = "请求超时。请重试。",
            ["message_unauthorized"] = "未授权。请登录。",
            ["message_forbidden"] = "访问被拒绝。",
            ["message_not_found"] = "未找到资源。",
            ["message_bad_request"] = "请求无效。请检查您的输入。",
            ["message_conflict"] = "资源已存在。",
            ["message_are_you_sure"] = "您确定吗?",
            ["message_confirmation_required"] = "需要确认",

            // 仪表板
            ["dashboard_title"] = "仪表板",
            ["dashboard_current_location"] = "当前位置",
            ["dashboard_no_default_location"] = "未设置默认位置。添加一个以开始。",
            ["dashboard_last_updated"] = "最后更新: {time}",
            ["dashboard_weather_data_unavailable"] = "天气数据目前不可用",

            // 验证
            ["validation_required"] = "此字段为必填项",
            ["validation_email_invalid"] = "无效的电子邮件地址",
            ["validation_password_too_short"] = "密码必须至少 8 个字符",
            ["validation_location_name_required"] = "需要地点名称",
            ["validation_coordinates_invalid"] = "无效的坐标",
            ["validation_temperature_invalid"] = "温度超出有效范围",
            ["validation_wind_speed_invalid"] = "风速超出有效范围",
            ["validation_update_frequency_invalid"] = "更新频率必须在 5 到 1440 分钟之间",
        };
    }
}

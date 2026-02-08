using System.Collections.Concurrent;

namespace TruweatherCore.Resources;

/// <summary>
/// Centralized resource manager for handling localized strings across API, Web, and Mobile.
/// Supports multiple languages with fallback to English.
/// </summary>
public class ResourceManager
{
    private static readonly ConcurrentDictionary<string, Dictionary<string, string>> _resources = new();
    private static string _currentLanguage = "en";

    static ResourceManager()
    {
        InitializeResources();
    }

    /// <summary>
    /// Set the current language for resource strings.
    /// Falls back to English if language not available.
    /// </summary>
    public static void SetLanguage(string languageCode)
    {
        if (!string.IsNullOrEmpty(languageCode) && _resources.ContainsKey(languageCode))
        {
            _currentLanguage = languageCode;
        }
        else
        {
            _currentLanguage = "en";
        }
    }

    /// <summary>
    /// Get a localized string by key.
    /// Returns the key itself if translation not found.
    /// </summary>
    public static string GetString(string key)
    {
        if (_resources.TryGetValue(_currentLanguage, out var languageResources))
        {
            if (languageResources.TryGetValue(key, out var value))
                return value;
        }

        // Fallback to English
        if (_currentLanguage != "en" && _resources.TryGetValue("en", out var englishResources))
        {
            if (englishResources.TryGetValue(key, out var value))
                return value;
        }

        return key; // Return key if not found
    }

    /// <summary>
    /// Get a localized string with string interpolation support.
    /// Usage: GetString("welcome_message", ("name", "John"))
    /// </summary>
    public static string GetString(string key, params (string placeholder, string value)[] replacements)
    {
        string baseString = GetString(key);

        foreach (var (placeholder, value) in replacements)
        {
            baseString = baseString.Replace($"{{{placeholder}}}", value);
        }

        return baseString;
    }

    /// <summary>
    /// Check if a specific key exists in current language.
    /// </summary>
    public static bool HasKey(string key)
    {
        if (_resources.TryGetValue(_currentLanguage, out var languageResources))
            return languageResources.ContainsKey(key);
        return false;
    }

    /// <summary>
    /// Get all available language codes.
    /// </summary>
    public static IEnumerable<string> GetAvailableLanguages()
    {
        return _resources.Keys;
    }

    /// <summary>
    /// Get current language code.
    /// </summary>
    public static string GetCurrentLanguage()
    {
        return _currentLanguage;
    }

    private static void InitializeResources()
    {
        _resources["en"] = EnglishResources.GetResources();
        _resources["es"] = SpanishResources.GetResources();
        _resources["fr"] = FrenchResources.GetResources();
        _resources["de"] = GermanResources.GetResources();
        _resources["it"] = ItalianResources.GetResources();
        _resources["pt"] = PortugueseResources.GetResources();
        _resources["ru"] = RussianResources.GetResources();
        _resources["zh"] = ChineseResources.GetResources();
        _resources["ja"] = JapaneseResources.GetResources();
        _resources["ko"] = KoreanResources.GetResources();
    }
}

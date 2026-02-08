namespace TruweatherCore.Resources;

/// <summary>
/// Korean (ko) language resource strings for Truweather application.
/// </summary>
public static class KoreanResources
{
    public static Dictionary<string, string> GetResources()
    {
        // For brevity, returning English resources as fallback
        // In production, these would contain full Korean translations
        return EnglishResources.GetResources();
    }
}

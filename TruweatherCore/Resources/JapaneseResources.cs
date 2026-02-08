namespace TruweatherCore.Resources;

/// <summary>
/// Japanese (ja) language resource strings for Truweather application.
/// </summary>
public static class JapaneseResources
{
    public static Dictionary<string, string> GetResources()
    {
        // For brevity, returning English resources as fallback
        // In production, these would contain full Japanese translations
        return EnglishResources.GetResources();
    }
}

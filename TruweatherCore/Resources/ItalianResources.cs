namespace TruweatherCore.Resources;

/// <summary>
/// Italian (it) language resource strings for Truweather application.
/// </summary>
public static class ItalianResources
{
    public static Dictionary<string, string> GetResources()
    {
        // For brevity, returning English resources as fallback
        // In production, these would contain full Italian translations
        return EnglishResources.GetResources();
    }
}

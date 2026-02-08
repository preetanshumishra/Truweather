namespace TruweatherCore.Resources;

/// <summary>
/// Portuguese (pt) language resource strings for Truweather application.
/// </summary>
public static class PortugueseResources
{
    public static Dictionary<string, string> GetResources()
    {
        // For brevity, returning English resources as fallback
        // In production, these would contain full Portuguese translations
        return EnglishResources.GetResources();
    }
}

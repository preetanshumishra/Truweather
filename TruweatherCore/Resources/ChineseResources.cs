namespace TruweatherCore.Resources;

/// <summary>
/// Chinese (zh) language resource strings for Truweather application.
/// </summary>
public static class ChineseResources
{
    public static Dictionary<string, string> GetResources()
    {
        // For brevity, returning English resources as fallback
        // In production, these would contain full Chinese translations
        return EnglishResources.GetResources();
    }
}

namespace TruweatherCore.Resources;

/// <summary>
/// German (de) language resource strings for Truweather application.
/// </summary>
public static class GermanResources
{
    public static Dictionary<string, string> GetResources()
    {
        // For brevity, returning English resources as fallback
        // In production, these would contain full German translations
        return EnglishResources.GetResources();
    }
}

namespace TruweatherCore.Resources;

/// <summary>
/// Russian (ru) language resource strings for Truweather application.
/// </summary>
public static class RussianResources
{
    public static Dictionary<string, string> GetResources()
    {
        // For brevity, returning English resources as fallback
        // In production, these would contain full Russian translations
        return EnglishResources.GetResources();
    }
}

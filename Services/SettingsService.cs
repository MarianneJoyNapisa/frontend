// Services/SettingsService.cs
using System.Text.Json;

public class SettingsService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public SettingsService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public AppSettings GetSettings()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null || httpContext.Session == null)
        {
            throw new InvalidOperationException("HTTP context or session is not available.");
        }
        var session = httpContext.Session;
        var settingsJson = session.GetString("AppSettings");
        
        if (string.IsNullOrEmpty(settingsJson))
        {
            return new AppSettings(); // Default settings
        }
        
        return JsonSerializer.Deserialize<AppSettings>(settingsJson) ?? new AppSettings();
    }

    public void SaveSettings(AppSettings settings)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null || httpContext.Session == null)
        {
            throw new InvalidOperationException("HTTP context or session is not available.");
        }
        var session = httpContext.Session;
        session.SetString("AppSettings", JsonSerializer.Serialize(settings));
    }
}

// Models/AppSettings.cs
public class AppSettings
{
    public bool DarkMode { get; set; } = false;
    public string PrimaryColor { get; set; } = "blue";
    public string FontSize { get; set; } = "medium";
    // Add other settings properties
}
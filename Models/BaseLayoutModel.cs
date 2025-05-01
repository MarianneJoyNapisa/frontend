// Models/BaseLayoutModel.cs
using Microsoft.AspNetCore.Mvc.RazorPages;

public class BaseLayoutModel(SettingsService settingsService) : PageModel
{
    protected readonly SettingsService _settingsService = settingsService;
    
    public AppSettings? Settings { get; set; }

    public virtual void OnGet()  // Make this virtual
    {
        Settings = _settingsService.GetSettings();
        ViewData["Settings"] = Settings;
    }
}
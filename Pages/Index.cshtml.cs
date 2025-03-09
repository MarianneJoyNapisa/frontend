using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeownersMS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var userIdentity = HttpContext.User.Identity;

            // Fix: Ensure Identity is not null before accessing its properties
            if (userIdentity == null || !userIdentity.IsAuthenticated)
            {
                // Redirect unauthenticated users to /Account/Login
                return RedirectToPage("/Account/Login");
            }

            // Redirect authenticated users to /Dashboard/Index (existing logic)
            return RedirectToPage("/Dashboard/Index");
        }
    }
}

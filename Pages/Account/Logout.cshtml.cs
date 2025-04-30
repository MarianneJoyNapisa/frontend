using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeownersMS.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear the authentication cookie
            HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");

            // Clear TempData to prevent leftover toast messages
            TempData["ToastMessage"] = null;

            // Redirect to the login page or index page
            return RedirectToPage("/Account/Login");
        }
    }
}

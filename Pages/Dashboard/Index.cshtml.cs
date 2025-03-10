using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace HomeownersMS.Pages.Dashboard
{
    [Authorize] // Ensure only authenticated users can access this page
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Get the user's role from the claims
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Redirect based on the user's role
            switch (userRole)
            {
                case "resident":
                    return RedirectToPage("/Dashboard/IndexResident");
                case "staff":
                    return RedirectToPage("/Dashboard/IndexStaff");
                case "admin":
                    return RedirectToPage("/Dashboard/IndexAdmin");
                default:
                    // If the role is not recognized, redirect to a default page or show an error
                    return RedirectToPage("/Account/AccessDenied"); // Example: Access denied page
            }
        }
    }
}


// Comment: Ram Railey Alin
// Index.cs is responsible for redirecting users to the appropriate dashboard page based on their role.
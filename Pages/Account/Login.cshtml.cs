using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using HomeownersMS.Data;
using HomeownersMS.Models;
using System.Security.Claims;

namespace HomeownersMS.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public LoginModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LoginInputModel LoginInput { get; set; } = new LoginInputModel();

        public class LoginInputModel
        {
            [Required]
            public string Username { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Clear the toast message to prevent it from appearing on refresh or after logout
            TempData["ToastMessage"] = null;

            // Reset the LoginInput model to clear the input fields
            LoginInput = new LoginInputModel();

            // Check if the user is already authenticated
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // Redirect authenticated users to the Index page
                return RedirectToPage("/Index");
            }
        
            return Page(); // Render the login page for unauthenticated users
        }


        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if(string.IsNullOrWhiteSpace(LoginInput.Username)){
                // If the username is empty, add a model error and return to the page to display the error message.
                ModelState.AddModelError(string.Empty, "Username is required.");
                return Page();
            }
            
            if(string.IsNullOrWhiteSpace(LoginInput.Password)){
                // If the password is empty, add a model error and return to the page to display the error message.
                ModelState.AddModelError(string.Empty, "Password is required.");
                return Page();
            }
            
            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Login failed. Please try again.";
                return Page();
            }

            // Check if the user exists in the database
            // Use the _context to query the database for the user with the provided username and check if the password is correct.
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == LoginInput.Username);
            
            if (user == null)
            {
                // If the user is not found, we can still return a generic error message
                // to prevent user enumeration attacks.
                ModelState.AddModelError("LoginInput.Username", "User not found.");
                TempData["ToastMessage"] = "Login failed. Please try again.";
                return Page();

            } else if (!user.VerifyPassword(LoginInput.Password))
            {
                // If the password is incorrect, we can still return a generic error message
                // to prevent user enumeration attacks.
                ModelState.AddModelError("LoginInput.Password", "Password is incorrect.");
                TempData["ToastMessage"] = "Login failed. Please try again.";
                return Page();
            }

            // If login is successful 
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Privilege.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = true }
            );

            TempData["ToastMessage"] = "Login successful.";
            return RedirectToPage("/Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeownersMS.Data;
using HomeownersMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeownersMS.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public RegisterModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = new User();

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool userExists = await _context.User
                .AnyAsync(u => u.Username == User.Username);

            if (userExists)
            {
                ModelState.AddModelError("User.Username", "Username is already taken.");
                return Page();
            }
            if (User.PasswordHash != ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return Page();
            }

            User.SetPassword(User.PasswordHash);

            _context.User.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}

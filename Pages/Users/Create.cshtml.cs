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

namespace HomeownersMS.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public CreateModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
        ViewData["AdminId"] = new SelectList(_context.Set<Admin>(), "AdminId", "FName");
        ViewData["ResidentId"] = new SelectList(_context.Set<Resident>(), "ResidentId", "FName");
        ViewData["StaffId"] = new SelectList(_context.Set<Staff>(), "StaffId", "FName");
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingUser = await _context.User
                .FirstOrDefaultAsync(u => u.Username == User.Username);

            if (existingUser != null)
            {
                ModelState.AddModelError("User.Username", "Username is already taken.");
                return Page();
            }

            if (User.PasswordHash != ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password do not match.");
                return Page();
            }

            if (!string.IsNullOrEmpty(User.PasswordHash))
            {
                User.SetPassword(User.PasswordHash);
            }

            _context.User.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

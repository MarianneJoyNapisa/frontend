using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Data;
using HomeownersMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages.Admin.Users
{
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public EditModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User UserList { get; set; } = default!;

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userIdentity = HttpContext.User.Identity;

            // Fix: Ensure Identity is not null before accessing its properties
            if (userIdentity == null || !userIdentity.IsAuthenticated || !HttpContext.User.IsInRole("admin"))
            {
                return RedirectToPage("/Account/AccessDenied");
            }

            if (id == null)
            {
                return NotFound();
            }

            var user =  await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            UserList = user;
            ViewData["AdminId"] = new SelectList(_context.Set<HomeownersMS.Models.Admin>(), "AdminId", "FName");
            ViewData["ResidentId"] = new SelectList(_context.Set<Resident>(), "ResidentId", "FName");
            ViewData["StaffId"] = new SelectList(_context.Set<Staff>(), "StaffId", "FName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UserList.PasswordHash != ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password do not match.");
                return Page();
            }

            // Fetch the existing user from the database
            var existingUser = await _context.Users.FindAsync(UserList.UserId);
            if (existingUser == null)
            {
                return NotFound();
            }

            // Update only the fields that have been explicitly set
            if (!string.IsNullOrEmpty(UserList.Username))
            {
                existingUser.Username = UserList.Username;
            }

            if (!string.IsNullOrEmpty(UserList.PasswordHash))
            {
                existingUser.SetPassword(UserList.PasswordHash);
            }

            if (UserList.Privilege.HasValue)
            {
                existingUser.Privilege = UserList.Privilege;
            }

            // Update navigation properties if needed
            if (UserList.Admin != null)
            {
                existingUser.Admin = UserList.Admin;
            }

            if (UserList.Staff != null)
            {
                existingUser.Staff = UserList.Staff;
            }

            if (UserList.Resident != null)
            {
                existingUser.Resident = UserList.Resident;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(UserList.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}

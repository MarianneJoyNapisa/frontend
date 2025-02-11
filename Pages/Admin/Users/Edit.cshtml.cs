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
        public User User { get; set; } = default!;

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated || !HttpContext.User.IsInRole("admin"))
            {
                return RedirectToPage("/Account/AccessDenied");
            }

            if (id == null)
            {
                return NotFound();
            }

            var user =  await _context.User.FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            User = user;
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

            if (User.PasswordHash != ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password do not match.");
                return Page();
            }

            if (!string.IsNullOrEmpty(User.PasswordHash))
            {
                User.SetPassword(User.PasswordHash);
            }

            _context.Attach(User).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(User.UserId))
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
            return _context.User.Any(e => e.UserId == id);
        }
    }
}

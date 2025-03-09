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

namespace HomeownersMS.Pages_Admin_Admins
{
    public class EditModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public EditModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Admin Admin { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin =  await _context.Admins.FirstOrDefaultAsync(m => m.UserId == id);
            if (admin == null)
            {
                return NotFound();
            }
            Admin = admin;
           ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
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

            var existingAdmin = await _context.Admins.FindAsync(Admin.UserId);

            if (existingAdmin != null)
            {
                // Remove the old admin
                _context.Admins.Remove(existingAdmin);
                await _context.SaveChangesAsync();
            }
            
            // Add the new admin with modified properties
            _context.Admins.Add(Admin);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle potential exceptions
                ModelState.AddModelError(string.Empty, $"Unable to save changes: {ex.Message}");
                return Page();
            }

            return RedirectToPage("./Index");
        }


        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.UserId == id);
        }
    }
}

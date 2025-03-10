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

namespace HomeownersMS.Pages_Admin_Residents
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
        public Resident Resident { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resident =  await _context.Residents.FirstOrDefaultAsync(m => m.UserId == id);
            if (resident == null)
            {
                return NotFound();
            }
            Resident = resident;
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

            var existingResident = await _context.Residents.FindAsync(Resident.UserId);

            if (existingResident != null)
            {
                // Remove the old admin
                _context.Residents.Remove(existingResident);
                await _context.SaveChangesAsync();
            }
            
            // Add the new admin with modified properties
            _context.Residents.Add(Resident);

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

        private bool ResidentExists(int id)
        {
            return _context.Residents.Any(e => e.UserId == id);
        }
    }
}

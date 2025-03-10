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

namespace HomeownersMS.Pages_Admin_Staffs
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
        public Staff Staff { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff =  await _context.Staffs.FirstOrDefaultAsync(m => m.UserId == id);
            if (staff == null)
            {
                return NotFound();
            }
            Staff = staff;
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

            var existingStaff = await _context.Staffs.FindAsync(Staff.UserId);

            if (existingStaff != null)
            {
                // Remove the old admin
                _context.Staffs.Remove(existingStaff);
                await _context.SaveChangesAsync();
            }
            
            // Add the new admin with modified properties
            _context.Staffs.Add(Staff);

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

        private bool StaffExists(int id)
        {
            return _context.Staffs.Any(e => e.UserId == id);
        }
    }
}

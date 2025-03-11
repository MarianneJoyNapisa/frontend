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
        [BindProperty]
        public IFormFile? ProfileImage { get; set; } // Property for the uploaded file

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
                Console.WriteLine("model not valid");
                return Page();
            }
            Console.WriteLine("model valid");


            var existingResident = await _context.Residents.Include(r => r.User)
                .FirstOrDefaultAsync(r => r.UserId == Resident.UserId);

            if (existingResident == null)
            {
                return NotFound();
            }

            // Update the properties of the existing resident
            existingResident.LName = Resident.LName;
            existingResident.FName = Resident.FName;
            existingResident.Email = Resident.Email;
            existingResident.ContactNo = Resident.ContactNo;
            existingResident.Address = Resident.Address;
            existingResident.MoveInDate = Resident.MoveInDate;

            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                // Define the folder to save the image
                var uploadsFolder = Path.Combine("wwwroot", "images", "profiles");

                // Ensure the folder exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(existingResident.ProfileImage))
                {
                    var oldImagePath = Path.Combine("wwwroot", existingResident.ProfileImage);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Generate a unique file name
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(fileStream);
                }

                // Save the new file path to the database (relative path)
                existingResident.ProfileImage = Path.Combine("images", "profiles", uniqueFileName);
            }
            
            // // Add the new admin with modified properties
            // _context.Residents.Add(Resident);

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

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

namespace HomeownersMS.Pages_Admin_Facilities
{
    [Authorize(Roles="admin")]

    public class EditModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public EditModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Facility Facility { get; set; } = default!;
        [BindProperty]
        public IFormFile? FacilityImage { get; set; } // Property for the uploaded file

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facility =  await _context.Facilities.FirstOrDefaultAsync(m => m.FacilityId == id);
            if (facility == null)
            {
                return NotFound();
            }
            Facility = facility;
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

            var existingFacility = await _context.Facilities.FindAsync(Facility.FacilityId);

            if (existingFacility == null)
            {
                return NotFound();
            }

            // Update text fields
            existingFacility.Name = Facility.Name;
            existingFacility.Description = Facility.Description;
            existingFacility.PricePerHour = Facility.PricePerHour;

            if (FacilityImage != null && FacilityImage.Length > 0)
            {
                // Delete old image if it exists
                if (!string.IsNullOrEmpty(existingFacility.FacilityImage))
                {
                    var oldImagePath = Path.Combine("wwwroot", existingFacility.FacilityImage);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Define folder for uploads
                var uploadsFolder = Path.Combine("wwwroot", "images", "facilities");

                // Ensure the folder exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique file name
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(FacilityImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the new image file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await FacilityImage.CopyToAsync(fileStream);
                }

                // Save new image path in the database
                existingFacility.FacilityImage = Path.Combine("images", "facilities", uniqueFileName);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacilityExists(Facility.FacilityId))
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

        private bool FacilityExists(int id)
        {
            return _context.Facilities.Any(e => e.FacilityId == id);
        }
    }
}

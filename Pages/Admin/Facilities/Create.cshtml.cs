using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeownersMS.Data;
using HomeownersMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages_Admin_Facilities
{
    [Authorize(Roles="admin")]

    public class CreateModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public CreateModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Facility Facility { get; set; } = default!;

        [BindProperty]
        public IFormFile? FacilityImage { get; set; } // Property for the uploaded file

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Handle file upload
            if (FacilityImage != null && FacilityImage.Length > 0)
            {
                // Define the folder to save the image
                var uploadsFolder = Path.Combine("wwwroot", "images", "facilities");

                // Ensure the folder exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate a unique file name
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + FacilityImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await FacilityImage.CopyToAsync(fileStream);
                }

                // Save the file path to the database
                Facility.FacilityImage = Path.Combine("images", "facilities", uniqueFileName);
            }

            _context.Facilities.Add(Facility);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

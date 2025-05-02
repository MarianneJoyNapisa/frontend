using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeownersMS.Data;
using Microsoft.AspNetCore.Authorization;
using HomeownersMS.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace HomeownersMS.Pages.Profile
{
    [Authorize(Roles = "resident,admin")]
    public class ProfileResidentModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        [BindProperty]
        public Resident? Resident { get; set; }

        [BindProperty]
        public IFormFile? ProfileImage { get; set; } // Property for the uploaded file

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null && int.TryParse(userId, out int residentId))
            {
                Resident = await _context.Residents
                    .FirstOrDefaultAsync(r => r.UserId == residentId) ?? throw new InvalidOperationException("Resident not found.");

                if (Resident == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null && int.TryParse(userId, out int residentId))
            {
                var residentToUpdate = await _context.Residents
                    .FirstOrDefaultAsync(r => r.UserId == residentId);

                if (residentToUpdate == null)
                {
                    return NotFound();
                }

                // Update the editable fields
                residentToUpdate.FName = Resident.FName;
                residentToUpdate.LName = Resident.LName;
                residentToUpdate.Email = Resident.Email;
                residentToUpdate.ContactNo = Resident.ContactNo;
                residentToUpdate.Address = Resident.Address;
                Console.WriteLine(Resident.FName);
                Console.WriteLine(Resident.LName);
                Console.WriteLine(Resident.Email);

                // Handle profile image upload
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    try{
                        // Define the folder to save the image
                        var uploadsFolder = Path.Combine("wwwroot", "images", "profiles");

                        // Ensure the folder exists
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Delete the old image if it exists
                        if (!string.IsNullOrEmpty(residentToUpdate.ProfileImage))
                        {
                            var oldImagePath = Path.Combine("wwwroot", residentToUpdate.ProfileImage);
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
                        residentToUpdate.ProfileImage = Path.Combine("images", "profiles", uniqueFileName);
                        
                        Console.WriteLine(residentToUpdate.ProfileImage);
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = "An error occurred while uploading the profile image.";
                        Console.WriteLine(ex.Message);
                        return Page();
                    }
                }

                try
                { // Save changes to the database
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToPage();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResidentExists(Resident.UserId))
                    { // Check if the resident still exists
                        TempData["ErrorMessage"] = "The resident record no longer exists.";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return Page();
        }

        private bool ResidentExists(int id)
        {
            return _context.Residents.Any(e => e.UserId == id);
        }
    }
}
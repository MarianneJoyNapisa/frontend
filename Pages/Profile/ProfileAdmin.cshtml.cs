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
    [Authorize(Roles = "admin")]
    public class ProfileAdminModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        [BindProperty]
        public Models.Admin? Admin { get; set; }

        [BindProperty]
        public IFormFile? ProfileImage { get; set; } // Property for the uploaded file

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null && int.TryParse(userId, out int adminId))
            {
                Admin = await _context.Admins
                    .FirstOrDefaultAsync(r => r.UserId == adminId) ?? throw new InvalidOperationException("Admin not found.");

                if (Admin == null)
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
            if (userId != null && int.TryParse(userId, out int adminId))
            {
                var adminToUpdate = await _context.Admins
                    .FirstOrDefaultAsync(r => r.UserId == adminId);

                if (adminToUpdate == null || Admin == null)
                {
                    return NotFound();
                }

                // Update the editable fields
                adminToUpdate.FName = Admin.FName;
                adminToUpdate.LName = Admin.LName;
                adminToUpdate.Email = Admin.Email;
                adminToUpdate.ContactNo = Admin.ContactNo;
                adminToUpdate.Job = Admin.Job;
                Console.WriteLine(Admin.FName);
                Console.WriteLine(Admin.LName);
                Console.WriteLine(Admin.Email);

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
                        if (!string.IsNullOrEmpty(adminToUpdate.ProfileImage))
                        {
                            var oldImagePath = Path.Combine("wwwroot", adminToUpdate.ProfileImage);
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
                        adminToUpdate.ProfileImage = Path.Combine("images", "profiles", uniqueFileName);
                        
                        Console.WriteLine(adminToUpdate.ProfileImage);
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
                    if (!AdminExists(Admin.UserId))
                    { // Check if the admin still exists
                        TempData["ErrorMessage"] = "The admin record no longer exists.";
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

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.UserId == id);
        }
    }
}
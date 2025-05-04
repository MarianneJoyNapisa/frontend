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
    [Authorize(Roles = "staff,admin")]
    public class ProfileStaffModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        [BindProperty]
        public Models.Staff? Staff { get; set; }

        [BindProperty]
        public IFormFile? ProfileImage { get; set; } // Property for the uploaded file

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null && int.TryParse(userId, out int staffId))
            {
                Staff = await _context.Staffs
                    .FirstOrDefaultAsync(r => r.UserId == staffId) ?? throw new InvalidOperationException("Staff not found.");

                if (Staff == null)
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
            if (userId != null && int.TryParse(userId, out int staffId))
            {
                var staffToUpdate = await _context.Staffs
                    .FirstOrDefaultAsync(r => r.UserId == staffId);

                if (staffToUpdate == null || Staff == null)
                {
                    return NotFound();
                }

                // Update the editable fields
                staffToUpdate.FName = Staff.FName;
                staffToUpdate.LName = Staff.LName;
                staffToUpdate.Email = Staff.Email;
                staffToUpdate.ContactNo = Staff.ContactNo;
                Console.WriteLine(Staff.FName);
                Console.WriteLine(Staff.LName);
                Console.WriteLine(Staff.Email);

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
                        if (!string.IsNullOrEmpty(staffToUpdate.ProfileImage))
                        {
                            var oldImagePath = Path.Combine("wwwroot", staffToUpdate.ProfileImage);
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
                        staffToUpdate.ProfileImage = Path.Combine("images", "profiles", uniqueFileName);
                        
                        Console.WriteLine(staffToUpdate.ProfileImage);
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
                    if (!StaffExists(Staff.UserId))
                    { // Check if the staff still exists
                        TempData["ErrorMessage"] = "The staff record no longer exists.";
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

        private bool StaffExists(int id)
        {
            return _context.Staffs.Any(e => e.UserId == id);
        }
    }
}
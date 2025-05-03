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
    public class ProfileStaffModel : PageModel
    {
        private readonly HomeownersContext _context;

        public ProfileStaffModel(HomeownersContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Staff Staff { get; set; } = default!;

        [BindProperty]
        public IFormFile? ProfileImage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null && int.TryParse(userId, out int staffId))
            {
                Staff = await _context.Staffs
                    .FirstOrDefaultAsync(s => s.UserId == staffId) ?? throw new InvalidOperationException("Staff not found.");

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
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null && int.TryParse(userId, out int staffId))
            {
                var staffToUpdate = await _context.Staffs
                    .FirstOrDefaultAsync(s => s.UserId == staffId);

                if (staffToUpdate == null)
                {
                    return NotFound();
                }

                // Update editable fields (excluding Job and HireDate)
                staffToUpdate.FName = Staff.FName;
                staffToUpdate.LName = Staff.LName;
                staffToUpdate.Email = Staff.Email;
                staffToUpdate.ContactNo = Staff.ContactNo;

                // Handle profile image upload
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine("wwwroot", "images", "profiles");
                    
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(staffToUpdate.ProfileImage))
                    {
                        var oldImagePath = Path.Combine("wwwroot", staffToUpdate.ProfileImage);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save new image
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProfileImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(fileStream);
                    }

                    staffToUpdate.ProfileImage = Path.Combine("images", "profiles", uniqueFileName);
                }

                try
                {
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToPage();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staffToUpdate.UserId))
                    {
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
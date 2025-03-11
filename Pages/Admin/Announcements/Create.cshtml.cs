using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeownersMS.Data;
using HomeownersMS.Models;
using System.Security.Claims;

namespace HomeownersMS.Pages_Admin_Announcements
{
    public class CreateModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public CreateModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["CreatedBy"] = new SelectList(_context.Admins, "UserId", "UserId");
            return Page();
        }

        [BindProperty]
        public Announcement Announcement { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // Custom validation for Title
            if (string.IsNullOrWhiteSpace(Announcement.Title))
            {
                ModelState.AddModelError("Announcement.Title", "The Title field is required.");
            }
            else if (Announcement.Title.Length < 5)
            {
                ModelState.AddModelError("Announcement.Title", "The Title must be at least 5 characters long.");
            }

            // Custom validation for Content
            if (string.IsNullOrWhiteSpace(Announcement.Content))
            {
                ModelState.AddModelError("Announcement.Content", "The Content field is required.");
            }

            // Custom validation for EventDate
            if (Announcement.EventDate < DateTime.Now)
            {
                ModelState.AddModelError("Announcement.EventDate", "The Event Date must be in the future.");
            }

            // If the ModelState is invalid, return the page with validation errors
            // AHHHH gagu mao diay. kay wala mn nako giapil ang CreatedAt ug CreatedBy sa form, edi wow always invalid ang model state. so ako ni iexclude daan sa modelSTate
            if (!ModelState.IsValid)
            {
                // Log all validation errors
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state?.Errors?.Any() == true)
                    {
                        Console.WriteLine($"Validation error for {key}: {state.Errors.First().ErrorMessage}");
                    }
                    
                }
                // Repopulate the dropdown if the model state is invalid
                ViewData["CreatedBy"] = new SelectList(_context.Admins, "UserId", "UserId");
                Console.WriteLine("model invalid");
                return Page();
            }

            // Automatically set CreatedBy to the logged-in admin's UserId
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                Announcement.CreatedBy = int.Parse(userId);
                Console.WriteLine("userid null");
                // Load the Admin entity from the database
                var admin = await _context.Admins.FindAsync(Announcement.CreatedBy);
                if (admin != null)
                {
                    Announcement.Admin = admin; // Set the Admin navigation property
                }
            }

            // Automatically set CreatedAt to the current date and time
            Announcement.CreatedAt = DateTime.Now;

            // Log the values of CreatedBy and CreatedAt
            Console.WriteLine($"CreatedBy: {Announcement.CreatedBy}, CreatedAt: {Announcement.CreatedAt}");

            // Add the announcement to the database
            _context.Announcements.Add(Announcement);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }    
    }
}

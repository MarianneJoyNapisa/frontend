using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeownersMS.Data;
using HomeownersMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeownersMS.Pages.Admin.Services
{
    public class CreateModel : PageModel
    {
        private readonly HomeownersContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(HomeownersContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Models.Service Service { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Form submitted");

            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Model state is not valid");
                return Page();
            }

            // Check if the StaffId exists in the Staff table
            var staffExists = await _context.Staffs.AnyAsync(s => s.StaffId == Service.StaffId);
            if (!staffExists)
            {
                _logger.LogInformation("StaffId does not exist");
                ModelState.AddModelError("Service.StaffId", "The specified StaffId does not exist.");
                return Page();
            }

            _context.Services.Add(Service);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Service created successfully");
            return RedirectToPage("./Index");
        }
    }
}

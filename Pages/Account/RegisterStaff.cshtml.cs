using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeownersMS.Data;
using HomeownersMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;


namespace HomeownersMS.Pages.Account
{
    [Authorize]
    public class RegisterStaffModel : PageModel
    {
        private readonly HomeownersContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public RegisterStaffModel(HomeownersContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserRegistrationModel UserInput { get; set; } = new();

        public class UserRegistrationModel
        {
            [Required]
            public string Username { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Required]
            public string LName { get; set; } = string.Empty;

            [Required]
            public string FName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [Phone]
            public string ContactNo { get; set; } = string.Empty;

            [Required]
            public string Job { get; set; } = string.Empty;

            [Required]
            public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Check if the username is already taken
            if (await _context.Users.AnyAsync(u => u.Username == UserInput.Username))
            {
                ModelState.AddModelError("UserInput.Username", "Username is already taken.");
                return Page();
            }

            // Create the User entity
            var user = new User
            {
                Username = UserInput.Username,
                PasswordHash = _passwordHasher.HashPassword(new User(), UserInput.Password),
                Privilege = Privileges.staff
            };

            // Create the Staff entity
            var staff = new Staff
            {
                LName = UserInput.LName,
                FName = UserInput.FName,
                Email = UserInput.Email,
                ContactNo = UserInput.ContactNo,
                Job = UserInput.Job,
                HireDate = UserInput.HireDate,
                User = user
            };

            // Add and save to the database
            _context.Users.Add(user);
            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/Users/Index");
        }
    }
}
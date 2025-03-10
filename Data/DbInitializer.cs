using Microsoft.EntityFrameworkCore;
using HomeownersMS.Data;
using HomeownersMS.Models;
using System;
using System.Linq;

namespace HomeownersMS.Data
{
    public static class DbInitializer
    {
        public static void Initialize(HomeownersContext context)
        {
            // Apply any pending migrations
            context.Database.Migrate();

            // Check if the Users and Admin tables already contain data
            if (context.Users.Any() || context.Admins.Any())
            {
                return; // DB has already been seeded
            }

            // Seed a default user
            var user = new User
            {
                Username = "admin", // Provide a default username
                Privilege = Privileges.admin // Assuming you have an enum for privileges
            };

            user.SetPassword("adminPassword123");

            context.Users.Add(user);
            context.SaveChanges(); // Save the user to get the generated UserId

            // Seed the corresponding admin for that user
            var admin = new Admin
            {
                UserId = user.UserId, // Link to the newly created user
                User = user, // This is the fix: You need to associate the User object to Admin
                FName = "Admin", // Admin first name
                LName = "User", // Admin last name
                Email = "admin@example.com", // Admin email
                ContactNo = "1234567890", // Admin contact number
                Job = "Administrator", // Admin job
                HireDate = DateTime.Now // Admin hire date
            };

            context.Admins.Add(admin);
            context.SaveChanges(); // Save the admin record
        }
    }
}

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
            if (context.Users.Any() || context.Admins.Any() || context.Residents.Any() || context.Staffs.Any() || context.Services.Any() )
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
                HireDate = DateOnly.FromDateTime(DateTime.Now) // Admin hire date
            };

            context.Admins.Add(admin);
            context.SaveChanges(); // Save the admin record

            // ========== CREATE DUMMY RESIDENT ==========
            var residentUser = new User
            {
                Username = "resident",
                Privilege = Privileges.resident
            };
            residentUser.SetPassword("residentPassword123");

            context.Users.Add(residentUser);
            context.SaveChanges();

            var resident = new Resident
            {
                UserId = residentUser.UserId,
                User = residentUser,
                FName = "John",
                LName = "Doe",
                Email = "resident@example.com",
                ContactNo = "0987654321",
                Address = "123 Main St",
                MoveInDate = DateOnly.FromDateTime(DateTime.Now)
            };

            context.Residents.Add(resident);
            context.SaveChanges();

            // ========== CREATE DUMMY STAFF ==========
            var staffUser = new User
            {
                Username = "staff",
                Privilege = Privileges.staff
            };
            staffUser.SetPassword("staffPassword123");

            context.Users.Add(staffUser);
            context.SaveChanges();

            var staff = new Staff
            {
                UserId = staffUser.UserId,
                User = staffUser,
                FName = "Jane",
                LName = "Smith",
                Email = "staff@example.com",
                ContactNo = "09123456789",
                Job = StaffJob.Maintenance,
                HireDate = DateOnly.FromDateTime(DateTime.Now)
            };

            context.Staffs.Add(staff);
            context.SaveChanges();

            // ========== CREATE SERVICES ==========
            var services = new List<Service>
            {
                // Repairs and Maintenance
                new Service
                {
                    Title = "Plumbing Repairs",
                    Description = "Fixing leaks, clogs, and other plumbing issues.",
                    ServiceCategory = ServiceCategory.repairsAndMaintenance,
                    DayRange = "Mon-Fri",
                    AvailableTimeStart = new TimeOnly(6, 0), // 6:00 AM
                    AvailableTimeEnd = new TimeOnly(18, 0)  // 6:00 PM
                },
                new Service
                {
                    Title = "Carpentry and Custom Building",
                    Description = "Custom furniture, repairs, and woodwork.",
                    ServiceCategory = ServiceCategory.repairsAndMaintenance,
                    DayRange = "Mon-Fri",
                    AvailableTimeStart = new TimeOnly(6, 0),
                    AvailableTimeEnd = new TimeOnly(18, 0)
                },
                new Service
                {
                    Title = "Electrical Services",
                    Description = "Electrical repairs, installations, and maintenance.",
                    ServiceCategory = ServiceCategory.repairsAndMaintenance,
                    DayRange = "Mon-Fri",
                    AvailableTimeStart = new TimeOnly(6, 0),
                    AvailableTimeEnd = new TimeOnly(18, 0)
                },

                // Cleaning Services
                new Service
                {
                    Title = "House Cleaning",
                    Description = "General house cleaning services.",
                    ServiceCategory = ServiceCategory.cleaningServices,
                    DayRange = "Mon-Fri",
                    AvailableTimeStart = new TimeOnly(6, 0),
                    AvailableTimeEnd = new TimeOnly(18, 0)
                },
                new Service
                {
                    Title = "Laundry Services",
                    Description = "Washing, drying, and folding clothes.",
                    ServiceCategory = ServiceCategory.cleaningServices,
                    DayRange = "Mon-Fri",
                    AvailableTimeStart = new TimeOnly(6, 0),
                    AvailableTimeEnd = new TimeOnly(18, 0)
                },
                new Service
                {
                    Title = "Pet Cleaning",
                    Description = "Cleaning and grooming services for pets.",
                    ServiceCategory = ServiceCategory.cleaningServices,
                    DayRange = "Mon-Fri",
                    AvailableTimeStart = new TimeOnly(6, 0),
                    AvailableTimeEnd = new TimeOnly(18, 0)
                },

                // Landscaping Maintenance
                new Service
                {
                    Title = "Gardening",
                    Description = "Plant care, weeding, and landscaping.",
                    ServiceCategory = ServiceCategory.landscapingMaintenance,
                    DayRange = "Mon-Fri",
                    AvailableTimeStart = new TimeOnly(6, 0),
                    AvailableTimeEnd = new TimeOnly(18, 0)
                },
                new Service
                {
                    Title = "Outdoor Repairs",
                    Description = "Repairs for outdoor furniture and structures.",
                    ServiceCategory = ServiceCategory.landscapingMaintenance,
                    DayRange = "Mon-Fri",
                    AvailableTimeStart = new TimeOnly(6, 0),
                    AvailableTimeEnd = new TimeOnly(18, 0)
                },
                new Service
                {
                    Title = "Lights Installation",
                    Description = "Installing and maintaining outdoor lighting.",
                    ServiceCategory = ServiceCategory.landscapingMaintenance,
                    DayRange = "Mon-Fri",
                    AvailableTimeStart = new TimeOnly(6, 0),
                    AvailableTimeEnd = new TimeOnly(18, 0)
                },

                // Other Services
                new Service
                {
                    Title = "Pest Control",
                    Description = "Eliminating pests and ensuring a pest-free environment.",
                    ServiceCategory = ServiceCategory.otherServices,
                    DayRange = "Mon-Fri",
                    AvailableTimeStart = new TimeOnly(6, 0),
                    AvailableTimeEnd = new TimeOnly(18, 0)
                },
                new Service
                {
                    Title = "Moving & Setup Help",
                    Description = "Assistance with moving and setting up furniture.",
                    ServiceCategory = ServiceCategory.otherServices,
                    DayRange = "Mon-Fri",
                    AvailableTimeStart = new TimeOnly(6, 0),
                    AvailableTimeEnd = new TimeOnly(18, 0)
                },
                new Service
                {
                    Title = "Emergency Services",
                    Description = "24/7 emergency assistance for urgent needs.",
                    ServiceCategory = ServiceCategory.otherServices,
                    DayRange = "Mon-Fri",
                    AvailableTimeStart = new TimeOnly(6, 0),
                    AvailableTimeEnd = new TimeOnly(18, 0)
                }
            };

            context.Services.AddRange(services);
            context.SaveChanges();
        }
    }
}

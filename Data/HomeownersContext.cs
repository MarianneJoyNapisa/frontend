using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;

namespace HomeownersMS.Data
{
    public class HomeownersContext : DbContext
    {
        public HomeownersContext(DbContextOptions<HomeownersContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Resident> Residents { get; set; } = default!;
        public DbSet<Staff> Staffs { get; set; } = default!;
        public DbSet<Admin> Admins { get; set; } = default!;
        public DbSet<Service> Services { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Admin.IsSeeding = true; // Enable seeding mode

            var passwordHasher = new PasswordHasher<User>();

            var admin = new Admin
            {
                AdminId = 1,
                LName = "Smith",
                FName = "John",
                Email = "admin@example.com",
                ContactNo = "1234567890",
                Job = "System Administrator",
                HireDate = DateTime.Now
            };

            var adminUser = new User
            {
                UserId = 1,
                Username = "admin",
                PasswordHash = passwordHasher.HashPassword(new User(), "Admin123!"),
                Privilege = Privileges.admin,
                AdminId = admin.AdminId  // ✅ Only set foreign key
            };

            modelBuilder.Entity<Admin>().HasData(admin);
            modelBuilder.Entity<User>().HasData(adminUser);

            Admin.IsSeeding = false; // Disable seeding mode
        }

    }
}

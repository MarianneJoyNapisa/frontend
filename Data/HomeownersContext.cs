using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;

namespace HomeownersMS.Data
{
    public class HomeownersContext : DbContext
    {
        public HomeownersContext (DbContextOptions<HomeownersContext> options)
            : base(options)
        {
        }

        public DbSet<HomeownersMS.Models.User> User { get; set; } = default!;
        public DbSet<HomeownersMS.Models.Resident>Resident { get; set; } = default!;
        public DbSet<HomeownersMS.Models.Staff>Staff { get; set; } = default!;
        public DbSet<HomeownersMS.Models.Admin>Admin { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Resident)
                .WithOne()
                .HasForeignKey<User>(u => u.ResidentId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Staff)
                .WithOne()
                .HasForeignKey<User>(u => u.StaffId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Admin)
                .WithOne()
                .HasForeignKey<User>(u => u.AdminId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Resident>().ToTable("Resident");
            modelBuilder.Entity<Staff>().ToTable("Staff");
            modelBuilder.Entity<Admin>().ToTable("Admin");
        }
    }
}

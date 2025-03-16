using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;
using ELNET_HomeownersMS.Models; // Ensure this is the correct namespace

namespace HomeownersMS.Data
{
    public class HomeownersContext : DbContext
    {
        public HomeownersContext(DbContextOptions<HomeownersContext> options)
            : base(options)
        {
        }

        public DbSet<Announcement> Announcements { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Resident> Residents { get; set; } = default!;
        public DbSet<Staff> Staffs { get; set; } = default!;
        public DbSet<Admin> Admins { get; set; } = default!;
        public DbSet<Service> Services { get; set; } = default!;
        public DbSet<ServiceRequest> ServiceRequests { get; set; } = default!;
        public DbSet<Facility> Facilities { get; set; } = default!;
        public DbSet<FacilityReview> FacilityReviews { get; set; } = default!;
        public DbSet<FacilityRequest> FacilityRequests { get; set; } = default!;
        public DbSet<CommunityPost> CommunityPosts { get; set; } = default!;
        public DbSet<CommunityComment> CommunityComments { get; set; } = default!;
        public DbSet<Resource> Resources { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Admin)
                .WithOne(a => a.User)
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Staff)
                .WithOne(s => s.User)
                .HasForeignKey<Staff>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Resident)
                .WithOne(r => r.User)
                .HasForeignKey<Resident>(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Service>()
                .HasOne(s => s.Staff)
                .WithMany(st => st.Services)
                .HasForeignKey(s => s.StaffId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Resident)
                .WithMany(r => r.ServiceRequests)
                .HasForeignKey(sr => sr.RequestedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Service)
                .WithMany(s => s.ServiceRequests)
                .HasForeignKey(sr => sr.ServiceId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Facility>()
                .HasMany(f => f.FacilityReviews)
                .WithOne(r => r.Facility)
                .HasForeignKey(r => r.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FacilityRequest>()
                .HasOne(fr => fr.Resident)
                .WithMany()
                .HasForeignKey(fr => fr.ResidentId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<FacilityRequest>()
                .HasOne(fr => fr.Facility)
                .WithMany()
                .HasForeignKey(fr => fr.FacilityId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CommunityPost>()
                .HasOne(cp => cp.User)
                .WithMany()
                .HasForeignKey(cp => cp.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CommunityComment>()
                .HasOne(cc => cc.CommunityPost)
                .WithMany()
                .HasForeignKey(cc => cc.CommunityPostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Resource>()
                .HasOne(r => r.Admin)
                .WithMany()
                .HasForeignKey(r => r.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Announcement>()
                .HasOne(a => a.Admin)
                .WithMany()
                .HasForeignKey(a => a.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Resident>().ToTable("Resident");
            modelBuilder.Entity<Staff>().ToTable("Staff");
            modelBuilder.Entity<Admin>().ToTable("Admin");
            modelBuilder.Entity<Service>().ToTable("Service");
            modelBuilder.Entity<ServiceRequest>().ToTable("ServiceRequest");
            modelBuilder.Entity<Facility>().ToTable("Facility");
            modelBuilder.Entity<FacilityRequest>().ToTable("FacilityRequest");
            modelBuilder.Entity<CommunityPost>().ToTable("CommunityPost");
            modelBuilder.Entity<CommunityComment>().ToTable("CommunityComment");
            modelBuilder.Entity<Announcement>().ToTable("Announcement");
            modelBuilder.Entity<Resource>().ToTable("Resource");
        }
    }
}

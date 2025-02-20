using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using HomeownersMS.Data;
using HomeownersMS.Services;

namespace HomeownersMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database Context (Only register once!)
            builder.Services.AddDbContext<HomeownersContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("HomeownersContext") 
                    ?? throw new InvalidOperationException("Connection string 'HomeownersContext' not found.")));

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Register UserService
            builder.Services.AddScoped<UserService>();

            // HTTP Context Accessor
            builder.Services.AddHttpContextAccessor();

            // Authentication and Authorization
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                });

            builder.Services.AddAuthorization();

            // Add Exception Filter for Dev Mode
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            // Database Initialization (Use Migrate() instead of EnsureCreated() if using Migrations)
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<HomeownersContext>();
                context.Database.Migrate(); // Apply pending migrations automatically
                DbInitializer.Initialize(context);
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}

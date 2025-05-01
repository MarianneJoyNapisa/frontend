using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using HomeownersMS.Data;
using HomeownersMS.Services;
using HomeownersMS.Hubs;

namespace HomeownersMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container in this order:

            // 1. Core services first
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();
            builder.Services.AddHttpContextAccessor();

            // 2. Database Context (correctly placed)
            builder.Services.AddDbContext<HomeownersContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("HomeownersContext")
                    ?? throw new InvalidOperationException("Connection string 'HomeownersContext' not found.")));

            // 3. Application services
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<SettingsService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            // 4. Authentication (simplified)
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true; // Added for better UX
                });

            // 5. Authorization
            builder.Services.AddAuthorization(options =>
            {
                // Add policies here if needed
                // options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            // 6. Session (with better security)
            builder.Services.AddSession(options => 
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Important for production
            });

            // 7. Development services
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            var app = builder.Build();

            // Configure the HTTP request pipeline in this order:

            // 1. Exception handling
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts(); // HTTP Strict Transport Security
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            // 2. Database initialization (correct placement)
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<HomeownersContext>();
                context.Database.Migrate();
                DbInitializer.Initialize(context);
            }

            // 3. Security and static files
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Cache static files for 1 week
                    ctx.Context.Response.Headers.Append(
                        "Cache-Control", "public,max-age=604800");
                }
            });

            // 4. Routing middleware
            app.UseRouting();

            // 5. Authentication & Authorization (must be after UseRouting)
            app.UseAuthentication();
            app.UseAuthorization();

            // 6. Session middleware (after auth, before endpoints)
            app.UseSession();

            // 7. Endpoints
            app.MapHub<NotificationHub>("/notificationHub");
            app.MapRazorPages();

            app.Run();
        }
    }
}


/*
Scaffold CRUD pages:
dotnet aspnet-codegenerator razorpage -m Staff -dc HomeownersContext -udl -outDir Pages/Admin/Staffs
                                    -> Model      -> DbContext                     -> Pages Route

Query Database:
sqlcmd -S "(localdb)\MSSQLLocalDB" -E
USE [HomeownersContext-e71be176-bc2e-4bde-b917-685163f5749a];
            -> Database name
SELECT * FROM [Staffs];
GO

SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE';

INSERT INTO [User] (Username, PasswordHash, Privilege)        
VALUES ('admin', 'qwerty', 2);      
go



dotnet ef migrations add MigrationName
dotnet ef database update

dotnet ef database drop --force
dotnet ef migrations remove
dotnet ef migrations list


initiate database:
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design

check sa C:\Users\<name sa imo user>\.dotnet\tools\dotnet-ef.exe
copy dir e.g. C:\Users\xx203\.dotnet\tools\dotnet-ef.exe

C:\Users\xx203\.dotnet\tools\dotnet-ef.exe migrations add InitialCreate
C:\Users\xx203\.dotnet\tools\dotnet-ef.exe database update

dotnet ef migrations add InitialCreate
dotnet ef database update


sqlite:
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design

pagination:
dotnet add package X.PagedList.Mvc.Core

dotnet add package X.PagedList --version 8.4.0
dotnet add package X.PagedList.Mvc.Core --version 8.4.0

dotnet add package Microsoft.AspNetCore.SignalR.Client

dotnet list package

git operations:

git checkout stash@{3} -- .   // forcefully checks out a stash. number is number of stash
git stash clear             // delete all stash


*/



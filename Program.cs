using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HomeownersMS.Data;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authentication.Cookies;
using HomeownersMS.Services;
namespace HomeownersMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<HomeownersContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("HomeownersContext") ?? throw new InvalidOperationException("Connection string 'HomeownersContext' not found.")));

            // Add services to the container.
            builder.Services.AddRazorPages();

            // UserService.cs
            builder.Services.AddScoped<UserService>();

            builder.Services.AddDbContext<HomeownersContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("HomeownersContext")));

            // Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Index";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            builder.Services.AddAuthorization();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Redirect unauthorized users to AccessDenied page
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<HomeownersContext>();
                context.Database.EnsureCreated();
                DbInitializer.Initialize(context);
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}

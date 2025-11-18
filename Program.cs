using ContractMonthlyClaimSystem_FINAL.Data;
using ContractMonthlyClaimSystem_FINAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContractMonthlyClaimSystem_FINAL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ========================================
            // 1. DATABASE + IDENTITY SETUP
            // ========================================
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? "Server=(localdb)\\mssqllocaldb;Database=CMCS_Final_DB;Trusted_Connection=True;MultipleActiveResultSets=true";

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Relaxed password rules for testing (you can tighten later)
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            // Note: .AddDefaultUI() removed. If you want the built-in Identity UI, install the
            // Microsoft.AspNetCore.Identity.UI package (via NuGet) and re-add .AddDefaultUI().

            // ========================================
            // 2. MVC + RAZOR PAGES
            // ========================================
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages(); // Needed for Identity UI (Login/Register)

            var app = builder.Build();

            // ========================================
            // 3. SEED ROLES & DEFAULT USERS
            // ========================================
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    await SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            // ========================================
            // 4. MIDDLEWARE PIPELINE
            // ========================================
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();   // MUST be before UseAuthorization
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages(); // Important for Identity pages

            app.Run();
        }
    }
}


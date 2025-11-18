using ContractMonthlyClaimSystem_FINAL.Models;
using ContractMonthlyClaimSystem_FINAL.Data;
using Microsoft.AspNetCore.Identity;

namespace ContractMonthlyClaimSystem_FINAL.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            string[] roles = { "Lecturer", "Coordinator", "Manager", "HR" };
            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            var lecturer = new ApplicationUser
            {
                UserName = "lecturer@cmcs.co.za",
                Email = "lecturer@cmcs.co.za",
                FullName = "John Doe",
                EmployeeId = "LEC001",
                Department = "Computer Science",
                HourlyRate = 450m,
                EmailConfirmed = true
            };

            if (await userManager.FindByEmailAsync(lecturer.Email) == null)
            {
                await userManager.CreateAsync(lecturer, "Pass@123");
                await userManager.AddToRoleAsync(lecturer, "Lecturer");
            }

            // Add other users...
        }
    }
}
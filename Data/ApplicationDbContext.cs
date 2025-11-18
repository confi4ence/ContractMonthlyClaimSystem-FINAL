using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ContractMonthlyClaimSystem_FINAL.Models;
namespace ContractMonthlyClaimSystem_FINAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Claim> Claims { get; set; } = null!;
    }
}

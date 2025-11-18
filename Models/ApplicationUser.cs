using Microsoft.AspNetCore.Identity;

namespace ContractMonthlyClaimSystem_FINAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = "LEC001";
        public string Department { get; set; } = "Academic";
        public decimal HourlyRate { get; set; } = 450.00m;
    }
}

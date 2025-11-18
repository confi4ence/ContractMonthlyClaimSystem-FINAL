using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ContractMonthlyClaimSystem_FINAL.Models
{
    public enum ClaimStatus
    {
        Pending, Verified, Approved, Rejected
    }

    public class Claim
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        [Required]
        public string Month { get; set; } = DateTime.Now.ToString("MMMM");
        [Required]
        public int Year { get; set; } = DateTime.Now.Year;

        [Required, Range(1, 200)]
        public decimal HoursWorked { get; set; }

        [Required, Range(100, 1000)]
        public decimal HourlyRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount => HoursWorked * HourlyRate;

        public string? SupportingDocumentPath { get; set; }
        public string? Notes { get; set; }

        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;
        public DateTime SubmittedDate { get; set; } = DateTime.Now;
        public DateTime? VerifiedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? RejectionReason { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractMonthlyClaimSystem_FINAL.Data;
using ContractMonthlyClaimSystem_FINAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContractMonthlyClaimSystem_FINAL.Controllers
{
    [Authorize(Roles = "Coordinator")]
    public class CoordinatorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoordinatorController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> PendingClaims()
        {
            var pending = await _context.Claims
                .Include(c => c.User)
                .Where(c => c.Status == ClaimStatus.Pending)
                .ToListAsync();
            return View(pending);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyClaim(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim is null)
            {
                TempData["Error"] = "Claim not found.";
                return RedirectToAction(nameof(PendingClaims));
            }

            claim.Status = ClaimStatus.Verified;
            claim.VerifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Claim verified.";
            return RedirectToAction(nameof(PendingClaims));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatchVerify([FromForm] List<int>? ids)
        {
            if (ids == null || ids.Count == 0)
            {
                TempData["Error"] = "No claims selected.";
                return RedirectToAction(nameof(PendingClaims));
            }

            var claims = await _context.Claims.Where(c => ids.Contains(c.Id)).ToListAsync();
            foreach (var c in claims)
            {
                c.Status = ClaimStatus.Verified;
                c.VerifiedDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"{claims.Count} claims verified!";
            return RedirectToAction(nameof(PendingClaims));
        }
    }
}

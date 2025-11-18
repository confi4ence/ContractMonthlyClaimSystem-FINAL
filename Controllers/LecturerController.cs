using ContractMonthlyClaimSystem_FINAL.Data;
using ContractMonthlyClaimSystem_FINAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Lecturer")]
public class ClaimsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _env;

    public ClaimsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
    {
        _context = context;
        _userManager = userManager;
        _env = env;
    }

    public IActionResult Index()
    {
        var userId = _userManager.GetUserId(User);
        var claims = _context.Claims.Where(c => c.UserId == userId).OrderByDescending(c => c.SubmittedDate).ToList();
        return View(claims);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Claim claim, IFormFile? document)
    {
        if (ModelState.IsValid)
        {
            claim.UserId = _userManager.GetUserId(User);
            claim.HourlyRate = (await _userManager.GetUserAsync(User))!.HourlyRate;

            if (document != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(document.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await document.CopyToAsync(stream);
                claim.SupportingDocumentPath = "/uploads/" + fileName;
            }

            _context.Add(claim);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Claim submitted successfully!";
            return RedirectToAction(nameof(Index));
        }
        return View(claim);
    }
}

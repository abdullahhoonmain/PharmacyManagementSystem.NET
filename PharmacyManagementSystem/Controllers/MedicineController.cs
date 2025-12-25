using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace PharmacyManagementSystem.Controllers
{
    public class MedicineController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMedicineAnalysisService _analysisService;

        public MedicineController(AppDbContext context, IMedicineAnalysisService analysisService)
        {
            _context = context;
            _analysisService = analysisService;
        }

        public async Task<IActionResult> Index()
        {
            var medicines = await _context.Medicines.ToListAsync();
            var badges = await _analysisService.GetMedicineBadgesAsync();
            ViewBag.Badges = badges.ToDictionary(b => b.MedicineId);
            return View(medicines);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                _context.Medicines.Add(medicine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicine);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return NotFound();
            return View(medicine);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Medicine medicine)
        {
            if (id != medicine.MedicineId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Medicines.Update(medicine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicine);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return NotFound();
            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
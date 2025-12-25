using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace PharmacyManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var accessLevel = HttpContext.Session.GetString("AccessLevel");
            if (accessLevel == null)
                return RedirectToAction("Login", "Auth");

            int level = int.Parse(accessLevel);
            ViewBag.AccessLevel = level;

            var bills = await _context.Bills.ToListAsync();
            var medicines = await _context.Medicines.ToListAsync();
            var customers = await _context.Customers.ToListAsync();
            var employees = await _context.Employees.ToListAsync();
            var suppliers = await _context.Suppliers.ToListAsync();

            ViewBag.TotalBills = bills.Count;
            ViewBag.TotalMedicines = medicines.Count;
            ViewBag.TotalCustomers = customers.Count;
            ViewBag.TotalEmployees = employees.Count;
            ViewBag.TotalSuppliers = suppliers.Count;

            var lowStockMedicines = medicines.Where(m => m.Quantity < 10).ToList();
            ViewBag.LowStockCount = lowStockMedicines.Count;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
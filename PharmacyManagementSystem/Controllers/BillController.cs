using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
namespace PharmacyManagementSystem.Controllers
{
    public class BillController : Controller
    {
        private readonly AppDbContext _context;

        public BillController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bills = await _context.Bills.Include(b => b.Customer).Include(b => b.Employee).ToListAsync();
            return View(bills);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = await _context.Customers.ToListAsync();
            ViewBag.Employees = await _context.Employees.ToListAsync();
            ViewBag.Medicines = await _context.Medicines.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Bill bill, int[] MedicineIds, int[] Quantities)
        {
            if (MedicineIds == null || MedicineIds.Length == 0)
            {
                ModelState.AddModelError("", "Add at least one medicine");
                ViewBag.Customers = await _context.Customers.ToListAsync();
                ViewBag.Employees = await _context.Employees.ToListAsync();
                ViewBag.Medicines = await _context.Medicines.ToListAsync();
                return View(bill);
            }

            if (ModelState.IsValid)
            {
                bill.BillDate = DateTime.Now;
                bill.BillItems = new List<BillDetails>();
                float totalAmount = 0;

                for (int i = 0; i < MedicineIds.Length; i++)
                {
                    var medicine = await _context.Medicines.FindAsync(MedicineIds[i]);
                    if (medicine != null && Quantities[i] > 0)
                    {
                        var billItem = new BillDetails
                        {
                            MedId = MedicineIds[i],
                            Quantity = Quantities[i],
                            UnitPrice = medicine.SalePrice,
                            Total = Quantities[i] * medicine.SalePrice
                        };
                        bill.BillItems.Add(billItem);
                        totalAmount += billItem.Total;

                        medicine.Quantity -= Quantities[i];  // ← Stock update karo
                        _context.Medicines.Update(medicine);
                    }
                }

                bill.TotalAmount = totalAmount;
                _context.Bills.Add(bill);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Customers = await _context.Customers.ToListAsync();
            ViewBag.Employees = await _context.Employees.ToListAsync();
            ViewBag.Medicines = await _context.Medicines.ToListAsync();
            return View(bill);
        }

        public async Task<IActionResult> Details(int id)
        {
            var bill = await _context.Bills
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.BillItems)
                .ThenInclude(bi => bi.Medicine)
                .FirstOrDefaultAsync(b => b.BillId == id);

            if (bill == null) return NotFound();
            return View(bill);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var bill = await _context.Bills.FindAsync(id);
            if (bill == null) return NotFound();
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

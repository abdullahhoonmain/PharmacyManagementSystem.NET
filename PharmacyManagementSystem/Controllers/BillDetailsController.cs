using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
namespace PharmacyManagementSystem.Controllers
{
    public class BillDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public BillDetailsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> BillItems(int billId)
        {
            var bill = await _context.Bills.FindAsync(billId);
            if (bill == null) return NotFound();

            var billItems = await _context.BillDetails.Where(bd => bd.BillId == billId).Include(bd => bd.Medicine).ToListAsync();
            ViewBag.BillId = billId;
            ViewBag.Bill = bill;
            return View(billItems);
        }

        public async Task<IActionResult> Create(int billId)
        {
            var bill = await _context.Bills.FindAsync(billId);
            if (bill == null) return NotFound();

            ViewBag.BillId = billId;
            ViewBag.Medicines = await _context.Medicines.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int billId, BillDetails billDetails)
        {
            billDetails.BillId = billId;

            if (ModelState.IsValid)
            {
                var medicine = await _context.Medicines.FindAsync(billDetails.MedId);
                if (medicine == null)
                {
                    ModelState.AddModelError("MedId", "Medicine not found");
                    ViewBag.BillId = billId;
                    ViewBag.Medicines = await _context.Medicines.ToListAsync();
                    return View(billDetails);
                }

                if (medicine.Quantity < billDetails.Quantity)
                {
                    ModelState.AddModelError("Quantity", $"Only {medicine.Quantity} units available");
                    ViewBag.BillId = billId;
                    ViewBag.Medicines = await _context.Medicines.ToListAsync();
                    return View(billDetails);
                }

                billDetails.UnitPrice = medicine.SalePrice;
                billDetails.Total = billDetails.Quantity * billDetails.UnitPrice;

                _context.BillDetails.Add(billDetails);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(BillItems), new { billId });
            }

            ViewBag.BillId = billId;
            ViewBag.Medicines = await _context.Medicines.ToListAsync();
            return View(billDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var billDetail = await _context.BillDetails.FindAsync(id);
            if (billDetail == null) return NotFound();

            ViewBag.Medicines = await _context.Medicines.ToListAsync();
            return View(billDetail);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BillDetails billDetails)
        {
            if (id != billDetails.BillItemId) return NotFound();

            if (ModelState.IsValid)
            {
                var medicine = await _context.Medicines.FindAsync(billDetails.MedId);
                if (medicine == null)
                {
                    ModelState.AddModelError("MedId", "Medicine not found");
                    ViewBag.Medicines = await _context.Medicines.ToListAsync();
                    return View(billDetails);
                }

                billDetails.UnitPrice = medicine.SalePrice;
                billDetails.Total = billDetails.Quantity * billDetails.UnitPrice;

                _context.BillDetails.Update(billDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(BillItems), new { billId = billDetails.BillId });
            }

            ViewBag.Medicines = await _context.Medicines.ToListAsync();
            return View(billDetails);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var billDetail = await _context.BillDetails.FindAsync(id);
            if (billDetail == null) return NotFound();

            int billId = billDetail.BillId;
            _context.BillDetails.Remove(billDetail);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(BillItems), new { billId });
        }
    }
}

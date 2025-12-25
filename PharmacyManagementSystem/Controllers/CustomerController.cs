using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace PharmacyManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        
            private readonly AppDbContext _context;

            public CustomerController(AppDbContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Index()
            {
                var customers = await _context.Customers.ToListAsync();
                return View(customers);
            }

            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(Customer customer)
            {
                if (ModelState.IsValid)
                {
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(customer);
            }

            public async Task<IActionResult> Edit(int id)
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null) return NotFound();
                return View(customer);
            }

            [HttpPost]
            public async Task<IActionResult> Edit(int id, Customer customer)
            {
                if (id != customer.CusId) return NotFound();
                if (ModelState.IsValid)
                {
                    _context.Customers.Update(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(customer);
            }

            public async Task<IActionResult> Delete(int id)
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null) return NotFound();
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        
    }
}

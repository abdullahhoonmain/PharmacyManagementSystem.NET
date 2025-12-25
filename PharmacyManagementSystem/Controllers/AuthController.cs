using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace PharmacyManagementSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.User
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("EmployeeId", user.EmployeeId.ToString());
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("AccessLevel", user.AccessLevel.ToString());
            HttpContext.Session.SetString("Role", user.Employee.Role);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(string username, string password, int employeeId, int accessLevel)
        {
            if (accessLevel < 1 || accessLevel > 3)
            {
                ModelState.AddModelError("", "Invalid access level (1-3)");
                return View();
            }

            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
            {
                ModelState.AddModelError("", "Employee not found");
                return View();
            }

            var existingUser = await _context.User
                .FirstOrDefaultAsync(u => u.Username == username);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Username already exists");
                return View();
            }

            var newUser = new User
            {
                Username = username,
                Password = password,
                EmployeeId = employeeId,
                AccessLevel = accessLevel
            };

            _context.User.Add(newUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
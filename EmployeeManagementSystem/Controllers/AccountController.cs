using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login page
        // GET: Login page
        public IActionResult Login()
        {
            // If already logged in, redirect to dashboard
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var role = HttpContext.Session.GetString("UserRole");
                if (role == "Admin")
                    return RedirectToAction("Index", "Admin");
                else
                    return RedirectToAction("Index", "Employee");
            }
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Email and password are required.";
                return View();
            }

            var user = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == email && e.Password == password && e.IsActive);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            // Set session
            HttpContext.Session.SetInt32("UserId", user.EmployeeId);
            HttpContext.Session.SetString("UserName", user.FirstName + " " + user.LastName);
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("UserEmail", user.Email);

            // Redirect based on role
            if (user.Role == "Admin")
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("Index", "Employee");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: Register (Only for creating first admin - can be removed later)
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var existingUser = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Email == employee.Email);

                if (existingUser != null)
                {
                    ViewBag.Error = "Email already exists.";
                    return View(employee);
                }

                employee.CreatedDate = DateTime.Now;
                employee.IsActive = true;

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }

            return View(employee);
        }
    }
}

using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Controllers
{
    public class SalaryBonusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalaryBonusController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        // GET: Bonus List
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var bonuses = await _context.SalaryBonuses
                .Include(s => s.Employee)
                .OrderByDescending(s => s.BonusDate)
                .ToListAsync();

            return View(bonuses);
        }

        // GET: Add Bonus
        public async Task<IActionResult> AddBonus()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Employees = await _context.Employees
                .Where(e => e.Role == "Employee" && e.IsActive)
                .ToListAsync();

            return View();
        }

        // POST: Add Bonus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBonus(SalaryBonus bonus)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                bonus.CreatedDate = DateTime.Now;
                _context.SalaryBonuses.Add(bonus);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Bonus added successfully!";
                return RedirectToAction("Index");
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Employees = await _context.Employees
                .Where(e => e.Role == "Employee" && e.IsActive)
                .ToListAsync();

            return View(bonus);
        }

        // GET: Edit Bonus
        public async Task<IActionResult> EditBonus(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var bonus = await _context.SalaryBonuses.FindAsync(id);
            if (bonus == null)
                return NotFound();

            ViewBag.Employees = await _context.Employees
                .Where(e => e.Role == "Employee" && e.IsActive)
                .ToListAsync();

            return View(bonus);
        }

        // POST: Edit Bonus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBonus(SalaryBonus bonus)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                _context.Update(bonus);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Bonus updated successfully!";
                return RedirectToAction("Index");
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Employees = await _context.Employees
                .Where(e => e.Role == "Employee" && e.IsActive)
                .ToListAsync();

            return View(bonus);
        }

        // POST: Delete Bonus
        [HttpPost]
        public async Task<IActionResult> DeleteBonus(int id)
        {
            if (!IsAdmin())
                return Json(new { success = false, message = "Unauthorized" });

            var bonus = await _context.SalaryBonuses.FindAsync(id);
            if (bonus == null)
                return Json(new { success = false, message = "Bonus not found" });

            _context.SalaryBonuses.Remove(bonus);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Bonus deleted successfully" });
        }
    }
}

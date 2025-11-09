using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        // GET: Feedback List (Admin)
        public async Task<IActionResult> Index(string status = "All")
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.SelectedStatus = status;

            var feedbacks = _context.Feedbacks.Include(f => f.Employee).AsQueryable();

            if (status != "All")
                feedbacks = feedbacks.Where(f => f.Status == status);

            var result = await feedbacks
                .OrderByDescending(f => f.SubmittedDate)
                .ToListAsync();

            return View(result);
        }

        // GET: View Feedback Details (Admin)
        public async Task<IActionResult> Details(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var feedback = await _context.Feedbacks
                .Include(f => f.Employee)
                .FirstOrDefaultAsync(f => f.FeedbackId == id);

            if (feedback == null)
                return NotFound();

            return View(feedback);
        }

        // POST: Update Feedback Status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status, string adminResponse)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
                return NotFound();

            feedback.Status = status;
            feedback.AdminResponse = adminResponse;

            _context.Update(feedback);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Feedback updated successfully!";
            return RedirectToAction("Details", new { id = id });
        }

        // POST: Delete Feedback
        [HttpPost]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            if (!IsAdmin())
                return Json(new { success = false, message = "Unauthorized" });

            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
                return Json(new { success = false, message = "Feedback not found" });

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Feedback deleted successfully" });
        }
    }
}

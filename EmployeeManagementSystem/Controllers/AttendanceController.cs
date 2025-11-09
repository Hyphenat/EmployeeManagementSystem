using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        // GET: Attendance List (Admin)
        public async Task<IActionResult> Index(DateTime? date)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            DateTime selectedDate = date ?? DateTime.Today;
            ViewBag.SelectedDate = selectedDate;

            var attendances = await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.Date.Date == selectedDate.Date)
                .OrderBy(a => a.Employee.FirstName)
                .ToListAsync();

            return View(attendances);
        }

        // GET: Mark Attendance
        public async Task<IActionResult> MarkAttendance(DateTime? date)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            DateTime selectedDate = date ?? DateTime.Today;
            ViewBag.SelectedDate = selectedDate;

            var employees = await _context.Employees
                .Where(e => e.Role == "Employee" && e.IsActive)
                .ToListAsync();

            var existingAttendance = await _context.Attendances
                .Where(a => a.Date.Date == selectedDate.Date)
                .ToListAsync();

            ViewBag.ExistingAttendance = existingAttendance;

            return View(employees);
        }

        // POST: Save Attendance
        [HttpPost]
        public async Task<IActionResult> SaveAttendance([FromBody] List<AttendanceDto> attendances)
        {
            if (!IsAdmin())
                return Json(new { success = false, message = "Unauthorized" });

            try
            {
                foreach (var att in attendances)
                {
                    var existing = await _context.Attendances
                        .FirstOrDefaultAsync(a => a.EmployeeId == att.EmployeeId && a.Date.Date == att.Date.Date);

                    if (existing != null)
                    {
                        existing.Status = att.Status;
                        existing.CheckInTime = att.CheckInTime;
                        existing.CheckOutTime = att.CheckOutTime;
                        existing.Remarks = att.Remarks;
                        _context.Update(existing);
                    }
                    else
                    {
                        var newAttendance = new Attendance
                        {
                            EmployeeId = att.EmployeeId,
                            Date = att.Date,
                            Status = att.Status,
                            CheckInTime = att.CheckInTime,
                            CheckOutTime = att.CheckOutTime,
                            Remarks = att.Remarks
                        };
                        _context.Attendances.Add(newAttendance);
                    }
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Attendance saved successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // GET: Employee Attendance Report
        public async Task<IActionResult> EmployeeReport(int id, int month, int year)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            ViewBag.Employee = employee;
            int selectedMonth = month == 0 ? DateTime.Now.Month : month;
            int selectedYear = year == 0 ? DateTime.Now.Year : year;
            ViewBag.Month = selectedMonth;
            ViewBag.Year = selectedYear;

            var attendances = await _context.Attendances
                .Where(a => a.EmployeeId == id && a.Date.Month == selectedMonth && a.Date.Year == selectedYear)
                .OrderBy(a => a.Date)
                .ToListAsync();

            return View(attendances);
        }
    }

    // DTO for attendance data
    public class AttendanceDto
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }
        public string? Remarks { get; set; }
    }
}
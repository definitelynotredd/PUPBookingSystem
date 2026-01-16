using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUPBookingSystem.Models;
using PUPBookingSystem.Data;

namespace PUPBookingSystem.Controllers
{
    // [Authorize(Roles = "Admin")] // <-- Keep this commented out while testing!
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // UPGRADE: Added 'filter' parameter to support the dashboard tabs
        public async Task<IActionResult> Index(string filter = "pending")
        {
            var query = _context.BookingRequests
                .Include(b => b.Room)
                .Include(b => b.User)
                .AsQueryable();

            // Apply Filter Logic
            switch (filter.ToLower())
            {
                case "pending":
                    query = query.Where(b => b.Status == "Pending");
                    break;
                case "approved":
                    query = query.Where(b => b.Status == "Approved");
                    break;
                case "rejected":
                    query = query.Where(b => b.Status == "Rejected");
                    break;
                default: // "all"
                    break;
            }

            // Sort by Date (Newest first)
            var requests = await query
                .OrderByDescending(b => b.Date)
                .ThenBy(b => b.StartTime)
                .ToListAsync();

            // Pass the filter to the View so we know which tab to highlight
            ViewBag.CurrentFilter = filter;

            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var req = await _context.BookingRequests.FindAsync(id);
            if (req != null)
            {
                req.Status = "Approved";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // UPGRADE: Temporarily removed 'reason' to match the simple Reject button in the UI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var req = await _context.BookingRequests.FindAsync(id);
            if (req != null)
            {
                req.Status = "Rejected";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
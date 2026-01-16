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

        // GET: Admin/Calendar
        public IActionResult Calendar()
        {
            // Fetch only APPROVED bookings
            var bookings = _context.BookingRequests
                .Include(b => b.Room)
                .Include(b => b.User)
                .Where(b => b.Status == "Approved")
                .Select(b => new
                {
                    id = b.Id,
                    title = b.Purpose,
                    room = b.Room.Code,
                    date = b.Date.ToString("yyyy-MM-dd"),
                    start = DateTime.Today.Add(b.StartTime).ToString("hh:mm tt"),
                    end = DateTime.Today.Add(b.EndTime).ToString("hh:mm tt"),
                    userName = b.User.Name
                })
                .ToList();

            // Pass data to view using ViewBag to serialize it later
            ViewBag.Bookings = bookings;

            return View();
        }
        // GET: Admin/Rooms
        public async Task<IActionResult> Rooms()
        {
            // Fetch all rooms from the database
            var rooms = await _context.Rooms.OrderBy(r => r.Id).ToListAsync();
            return View(rooms);
        }

        // POST: Admin/UpdateRoom
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRoom(int id, int capacity, string status, string hours, string notes)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            // Update the room details
            room.Capacity = capacity;
            room.Status = status;
            room.Hours = hours;
            room.Notes = notes;

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            // Go back to the Rooms list
            return RedirectToAction("Rooms");
        }
    }
}
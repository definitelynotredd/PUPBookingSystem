using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUPBookingSystem.Data;
using PUPBookingSystem.Models;
using System.Security.Claims;

namespace PUPBookingSystem.Controllers
{
    [Authorize] // <--- RESTORED: This locks the whole section. Simple and reliable.
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Room Directory
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rooms.ToListAsync());
        }

        // 2. Create Request (GET)
        [HttpGet]
        public async Task<IActionResult> Create(int? roomId)
        {
            if (roomId == null) return RedirectToAction("Index");

            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null) return NotFound();

            ViewBag.Room = room;

            return View(new BookingRequest { RoomId = room.Id, Date = DateTime.Today });
        }

        // 3. Create Request (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingRequest request)
        {
            ModelState.Remove("User");
            ModelState.Remove("Room");
            ModelState.Remove("UserId");

            if (request.StartTime >= request.EndTime)
                ModelState.AddModelError("", "End time must be after start time.");

            bool conflict = await _context.BookingRequests.AnyAsync(b =>
                b.RoomId == request.RoomId &&
                b.Date == request.Date &&
                b.Status == "Approved" &&
                b.StartTime < request.EndTime && request.StartTime < b.EndTime);

            if (conflict)
                ModelState.AddModelError("", "This time slot is already booked.");

            if (ModelState.IsValid)
            {
                request.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                request.Status = "Pending";
                request.CreatedAt = DateTime.Now;

                _context.BookingRequests.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyRequests");
            }

            var room = await _context.Rooms.FindAsync(request.RoomId);
            ViewBag.Room = room;
            return View(request);
        }

        // 4. My Requests
        public async Task<IActionResult> MyRequests()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var reqs = await _context.BookingRequests
                .Include(r => r.Room)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Date)
                .ToListAsync();

            return View(reqs);
        }

        // 5. Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.BookingRequests.FindAsync(id);
            if (booking != null && booking.Status == "Pending")
            {
                _context.BookingRequests.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("MyRequests");
        }
    }
}
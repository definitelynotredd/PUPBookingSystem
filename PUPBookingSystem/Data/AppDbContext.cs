using Microsoft.EntityFrameworkCore;
using PUPBookingSystem.Models;

namespace PUPBookingSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<BookingRequest> BookingRequests { get; set; }
        // public DbSet<RoomBlock> RoomBlocks { get; set; } // Comment this out if you don't have the class yet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var rooms = new List<Room>();

            // Loop 1 to 12. logic applies to EVERYONE.
            for (int i = 1; i <= 12; i++)
            {
                rooms.Add(new Room
                {
                    Id = i,
                    Code = $"S5{i:D2}",
                    Capacity = 50,              // New Capacity
                    Status = "Available",
                    Notes = "Standard Lab Room",
                    Hours = "7:00 AM - 9:00 PM" // Same hours for EVERYONE
                });
            }

            modelBuilder.Entity<Room>().HasData(rooms);

            base.OnModelCreating(modelBuilder);
        }
    }
}
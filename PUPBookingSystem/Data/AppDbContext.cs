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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var rooms = new List<Room>();

            // Loop 1 to 12.
            for (int i = 1; i <= 12; i++)
            {
                rooms.Add(new Room
                {
                    Id = i,
                    Code = $"S5{i:D2}",
                    Capacity = 50,              
                    Status = "Available",
                    Notes = "Standard Lab Room",
                    Hours = "7:00 AM - 9:00 PM" // Default operating hours
                });
            }

            modelBuilder.Entity<Room>().HasData(rooms);

            base.OnModelCreating(modelBuilder);
        }
    }
}
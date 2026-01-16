using Microsoft.EntityFrameworkCore;
using PUPBookingSystem.Models;

namespace PUPBookingSystem.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<BookingRequest> BookingRequests { get; set; }
    public DbSet<RoomBlock> RoomBlocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed Rooms S501 to S512
        var rooms = new List<Room>();
        for (int i = 1; i <= 12; i++)
        {
            rooms.Add(new Room
            {
                Id = i,
                Code = $"S5{i:D2}",
                // CHANGE THIS FROM 40 TO 50
                Capacity = 50,
                Status = "Available",
                Notes = "Standard Lab Room"
            });
        }
        modelBuilder.Entity<Room>().HasData(rooms);
        // ... rest of code
    }
}
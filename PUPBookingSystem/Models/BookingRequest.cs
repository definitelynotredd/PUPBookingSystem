using System.ComponentModel.DataAnnotations;

// This wrapper is what was missing!
namespace PUPBookingSystem.Models
{
    public class BookingRequest
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Required]
        public string Purpose { get; set; } = string.Empty; // Initialize it!

        // Added this to ensure compatibility with your new form
        public string? Organization { get; set; }

        public string? Description { get; set; }

        [Range(1, 100, ErrorMessage = "Attendees must be at least 1")]
        public int Attendees { get; set; }

        public string Status { get; set; } = "Pending";
        public string? AdminNote { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
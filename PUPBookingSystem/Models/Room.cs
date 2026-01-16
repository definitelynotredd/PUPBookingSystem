using System.ComponentModel.DataAnnotations;

namespace PUPBookingSystem.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; } = string.Empty; // Fixed Warning: Added default value

        public int Capacity { get; set; }

        public string Status { get; set; } = "Available";

        public string? Notes { get; set; } // Fixed Warning: Added '?' to make it optional

        public string? Description { get; set; }
    }
}
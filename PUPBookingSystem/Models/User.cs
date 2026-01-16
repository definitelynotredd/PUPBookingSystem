using System.ComponentModel.DataAnnotations;

namespace PUPBookingSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // Fixed: Initialized to empty string

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; // Fixed: Initialized to empty string

        [Required]
        public string PasswordHash { get; set; } = string.Empty; // Fixed: Initialized to empty string

        public string Role { get; set; } = "Student"; // Default role

        public string? Organization { get; set; } // Optional (nullable)
    }
}
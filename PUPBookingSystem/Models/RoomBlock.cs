using System.ComponentModel.DataAnnotations;

namespace PUPBookingSystem.Models // WRAP IT LIKE THE OTHERS
{
    public class RoomBlock
    {
        public int Id { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; } // Now it can find 'Room' easily

        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Reason { get; set; } = string.Empty; // Fixed warning too
    }
}
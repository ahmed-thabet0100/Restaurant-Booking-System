using Book.Data.Entities;
using Restaurant.Data.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Booking.Data.Entities
{
    public class Reservation : BaseEntity
    {

        public string UserId { get; set; }
        public string UserName { get; set; }

        public int RestaurantId { get; set; }
        public Book.Data.Entities.Restaurant Restaurant { get; set; }

        public int? TableNumber { get; set; }
        public Table Table { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public int NumberOfGuests { get; set; }
        public int BookNum { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        public string? AssignedBy { get; set; }
        public DateTime? AssignedAt { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool ReminderSend { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}

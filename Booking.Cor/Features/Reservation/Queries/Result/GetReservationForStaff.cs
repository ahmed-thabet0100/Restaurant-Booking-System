using Restaurant.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Cor.Features.Reservation.Queries.Result
{
    public class GetReservationForStaff
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int? TableNumber { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int NumberOfGuests { get; set; }
        public string Status { get; set; }
        public DateTime? AssignedAt { get; set; }
        public string? AssignedBy { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}

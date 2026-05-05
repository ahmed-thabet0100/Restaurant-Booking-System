using MediatR;
using Restaurant.Service;
using System.ComponentModel.DataAnnotations;

namespace Booking.Cor.Features.Reservation.Commands.Model
{
    public class CreateReservationCommand : IRequest<Response<CreateReservationCommand>>
    {
        public CreateReservationCommand(int restaurantId, DateTime startDateTime, DateTime endDateTime, int numberOfGuests, string? notes)
        {
            RestaurantId = restaurantId;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            NumberOfGuests = numberOfGuests;
            Notes = notes;
        }

        [Required]
        public int RestaurantId { get; set; }
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime StartDateTime { get; set; }
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime EndDateTime { get; set; }
        [Required]
        public int NumberOfGuests { get; set; }
        [DataType(DataType.Text)]
        public string? Notes { get; set; }

    }
}

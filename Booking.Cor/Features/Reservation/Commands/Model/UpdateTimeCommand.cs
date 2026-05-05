using Booking.Cor.Features.Reservation.Queries.Result;
using MediatR;
using Restaurant.Service;
using System.ComponentModel.DataAnnotations;

namespace Booking.Cor.Features.Reservation.Commands.Model
{
    public class UpdateTimeCommand : IRequest<Response<GetReservationForUser>>
    {

        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }


    }
}

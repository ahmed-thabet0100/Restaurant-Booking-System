using Booking.Cor.Features.Restaurant.Queries.Result;
using MediatR;
using Restaurant.Service;

namespace Booking.Cor.Features.Restaurant.Commands.Model
{
    public class AddReviewCommand : IRequest<Response<GetReviewQuery>>
    {
        public int RestaurantId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}

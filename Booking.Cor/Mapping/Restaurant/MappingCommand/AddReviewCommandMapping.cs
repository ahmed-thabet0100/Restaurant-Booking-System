using Booking.Cor.Features.Restaurant.Commands.Model;
using Booking.Data.Entities;

namespace Restaurant.Cor.Mapping.Student
{
    public partial class RestaurantProfile
    {
        public void AddReviewCommandMapping()
        {
            CreateMap<Review, AddReviewCommand>().ReverseMap();
        }
    }
}

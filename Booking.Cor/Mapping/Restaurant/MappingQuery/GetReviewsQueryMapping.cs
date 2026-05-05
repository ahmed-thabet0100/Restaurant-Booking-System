using Booking.Cor.Features.Restaurant.Queries.Result;
using Booking.Data.Entities;

namespace Restaurant.Cor.Mapping.Student
{
    public partial class RestaurantProfile
    {
        public void GetReviewsQueryMapping()
        {
            CreateMap<Review, GetReviewQuery>().ReverseMap();
        }
    }
}

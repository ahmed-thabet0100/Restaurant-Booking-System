using Booking.Data.Entities;
using Restaurant.Infra.Specifications.Bases;

namespace Booking.Infra.Specifications.ReviewSpecifications
{
    public class ReviewForCountSpecification : BaseSpecification<Review>
    {
        public ReviewForCountSpecification(ReviewSpecParams specParams)
            : base(r => r.RestaurantId == specParams.RestaurantId)
        {

        }
    }
}

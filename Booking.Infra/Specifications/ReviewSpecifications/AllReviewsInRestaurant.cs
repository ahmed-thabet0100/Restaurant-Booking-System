using Booking.Data.Entities;
using Restaurant.Infra.Specifications.Bases;

namespace Booking.Infra.Specifications.ReviewSpecifications
{
    public class AllReviewsInRestaurant : BaseSpecification<Review>
    {
        public AllReviewsInRestaurant(ReviewSpecParams specParams)
            : base(r => r.RestaurantId == specParams.RestaurantId)
        {
            //Includes.Add(r => r.User);

            ApplyPaging(
                specParams.PageSize * (specParams.PageIndex - 1),
                specParams.PageSize);

            AddOrderByDescending(r => r.CreatedAt);
        }
    }
}

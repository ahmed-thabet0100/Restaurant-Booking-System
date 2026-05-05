using Restaurant.Infra.Specifications.Bases;

namespace Booking.Infra.Specifications.RestaurantSpecifications
{
    public class GetRestaurantOwner : BaseSpecification<Book.Data.Entities.Restaurant>
    {
        public GetRestaurantOwner(string userId) :
            base(r => r.OwnerId == userId)
        {

        }
    }
}

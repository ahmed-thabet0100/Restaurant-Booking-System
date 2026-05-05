using Book.Data.Entities;
using Restaurant.Infra.Specifications.Bases;

namespace Restaurant.Infra.Specifications.RestaurantSpecifications
{
    public class RestaurantForCountSpecification
    : BaseSpecification<Book.Data.Entities.Restaurant>
    {
        public RestaurantForCountSpecification(RestaurantSpecParams param)
            : base(r =>
                string.IsNullOrEmpty(param.Search) ||
                r.Name.ToLower().Contains(param.Search.ToLower()))
        {
        }
    }

}

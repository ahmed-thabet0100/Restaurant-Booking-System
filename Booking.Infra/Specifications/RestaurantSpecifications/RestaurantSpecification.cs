using Book.Data.Entities;
using Restaurant.Infra.Specifications.Bases;

namespace Restaurant.Infra.Specifications.RestaurantSpecifications
{
    public class RestaurantSpecification : BaseSpecification<Book.Data.Entities.Restaurant>
    {
        public RestaurantSpecification(RestaurantSpecParams param)
                : base(r =>
                    string.IsNullOrEmpty(param.Search) ||
                    r.Name.ToLower().Contains(param.Search.ToLower()))
        {
            // Sorting
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDescending(r => r.Name);
                        break;

                    default:
                        AddOrderBy(r => r.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(r => r.Name);
            }

            // Pagination
            ApplyPaging(
                param.PageSize * (param.PageIndex - 1),
                param.PageSize);
        }
    }

}

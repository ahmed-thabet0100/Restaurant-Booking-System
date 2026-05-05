using Book.Data.Entities;
using Restaurant.Infra.Specifications.Bases;

namespace Restaurant.Infra.Specifications.TableSpecifications
{
    public class OneTableInRestaurantSpecification : BaseSpecification<Table>
    {
        public OneTableInRestaurantSpecification(int TableNum, int RestaurantId)
            : base(t => t.TableNumber == TableNum && t.RestaurantId == RestaurantId)
        {
            AddInclude(t => t.Restaurant);
        }
    }
}

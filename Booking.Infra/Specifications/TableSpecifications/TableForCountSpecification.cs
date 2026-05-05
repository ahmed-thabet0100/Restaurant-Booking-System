using Book.Data.Entities;
using Restaurant.Infra.Specifications.Bases;

namespace Restaurant.Infra.Specifications.TableSpecifications
{
    public class TableForCountSpecification : BaseSpecification<Table>
    {
        public TableForCountSpecification(TableSpecParams param, int RestaurantId) :
            base(t =>
            t.RestaurantId == RestaurantId &&
            (!param.Capacity.HasValue || t.Capacity >= param.Capacity))
        {

        }
    }
}

using Book.Data.Entities;
using Restaurant.Infra.Specifications.Bases;

namespace Restaurant.Infra.Specifications.TableSpecifications
{

    public class AllTablesInRestaurantSpecification
        : BaseSpecification<Table>
    {
        public AllTablesInRestaurantSpecification(
            TableSpecParams specParams, int RestaurantId)
            : base(t =>
                t.RestaurantId == RestaurantId &&
                (!specParams.Capacity.HasValue
                    || t.Capacity >= specParams.Capacity))
        {
            AddInclude(t => t.Restaurant);

            AddOrderBy(t => t.TableNumber);

            ApplyPaging(
                specParams.PageSize * (specParams.PageIndex - 1),
                specParams.PageSize);

            ApplySorting(specParams);
        }

        private void ApplySorting(TableSpecParams specParams)
        {
            switch (specParams.Sort)
            {
                case "capacityAsc":
                    AddOrderBy(t => t.Capacity);
                    break;

                case "capacityDesc":
                    AddOrderByDescending(t => t.Capacity);
                    break;

                case "numberDesc":
                    AddOrderByDescending(t => t.TableNumber);
                    break;

                default:
                    AddOrderBy(t => t.TableNumber);
                    break;
            }
        }
    }
}

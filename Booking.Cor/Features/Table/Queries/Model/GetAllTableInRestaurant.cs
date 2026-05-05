using MediatR;
using Restaurant.Cor.Bases;
using Restaurant.Cor.Features.Table.Queries.Result;
using Restaurant.Infra.Specifications.TableSpecifications;
using Restaurant.Service;

namespace Restaurant.Cor.Features.Table.Queries.Model
{
    public class GetAllTableInRestaurant : IRequest<Response<Pagination<GetTableDto>>>
    {
        public GetAllTableInRestaurant(TableSpecParams specParams)
        {
            SpecParams = specParams;
        }
        public TableSpecParams SpecParams { get; set; }
    }
}

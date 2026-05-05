using MediatR;
using Restaurant.Cor.Bases;
using Restaurant.Cor.Features.Restaurant.Queries.Result;
using Restaurant.Infra.Specifications.RestaurantSpecifications;
using Restaurant.Service;

namespace Restaurant.Cor.Features.Restaurant.Queries.Model
{
    public class GetAllRestaurantListQuery
        : IRequest<Response<Pagination<ReturnRestaurantQuery>>>
    {
        public GetAllRestaurantListQuery(RestaurantSpecParams specParams)
        {
            SpecParams = specParams;
        }

        public RestaurantSpecParams SpecParams { get; set; }
    }
}

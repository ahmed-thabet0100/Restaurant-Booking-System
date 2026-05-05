using MediatR;
using Restaurant.Cor.Features.Restaurant.Queries.Result;
using Restaurant.Service;

namespace Restaurant.Cor.Features.Restaurant.Queries.Model
{
    public class GetRestaurantByIdQuery : IRequest<Response<ReturnRestaurantQuery>>
    {
        public int Id { get; set; }
        public GetRestaurantByIdQuery(int id)
        {
            Id = id;
        }
    }
}

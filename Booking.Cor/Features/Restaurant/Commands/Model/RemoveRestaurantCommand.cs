using MediatR;
using Restaurant.Service;

namespace Restaurant.Cor.Features.Restaurant.Commands.Model
{
    public class RemoveRestaurantCommand : IRequest<Response<string>>
    {
    }
}

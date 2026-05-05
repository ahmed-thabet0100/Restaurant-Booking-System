using Booking.Cor.Features.Restaurant.Queries.Result;
using Booking.Infra.Specifications.ReviewSpecifications;
using MediatR;
using Restaurant.Cor.Bases;
using Restaurant.Service;

namespace Booking.Cor.Features.Restaurant.Queries.Model
{
    public class GetAllReviewsInRestaurantQuery : IRequest<Response<Pagination<GetReviewQuery>>>
    {
        public ReviewSpecParams specParams;

        public GetAllReviewsInRestaurantQuery(ReviewSpecParams specParams)
        {
            this.specParams = specParams;
        }
    }
}

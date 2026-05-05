using AutoMapper;
using Booking.Cor.Features.Restaurant.Queries.Model;
using Booking.Cor.Features.Restaurant.Queries.Result;
using MediatR;
using Restaurant.Cor.Bases;
using Restaurant.Cor.Features.Restaurant.Queries.Model;
using Restaurant.Cor.Features.Restaurant.Queries.Result;
using Restaurant.Service;
using Restaurant.Service.Abstracts;

namespace Restaurant.Cor.Features.Restaurant.Queries.Handeler
{
    public class RestaurantQueryHandeler : IRequestHandler<GetRestaurantByIdQuery, Response<ReturnRestaurantQuery>>,
                                           IRequestHandler<GetAllRestaurantListQuery, Response<Pagination<ReturnRestaurantQuery>>>,
                                           IRequestHandler<GetAllReviewsInRestaurantQuery, Response<Pagination<GetReviewQuery>>>
    {
        private readonly IRestaurantService _service;
        private readonly IMapper _mapper;
        private readonly ResponseHandler _responseHandler;

        public RestaurantQueryHandeler(IRestaurantService service, IMapper mapper, ResponseHandler responseHandler)
        {
            _service = service;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }
        public async Task<Response<Pagination<ReturnRestaurantQuery>>> Handle(
            GetAllRestaurantListQuery request,
            CancellationToken cancellationToken)
        {
            var (restaurants, totalItems) = await _service.GetAllRestaurantsAsync(request.SpecParams);

            if (restaurants == null || !restaurants.Any())
                return _responseHandler.NotFound<Pagination<ReturnRestaurantQuery>>("No restaurants found");

            var data = _mapper.Map<IReadOnlyList<ReturnRestaurantQuery>>(restaurants);

            var pagination = new Pagination<ReturnRestaurantQuery>(
                request.SpecParams.PageIndex,
                request.SpecParams.PageSize,
                totalItems,
                data
            );

            return _responseHandler.Success(pagination);
        }
        public async Task<Response<ReturnRestaurantQuery>> Handle(
            GetRestaurantByIdQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _service.GetRestaurantAsyncByIdAsync(request.Id);

            if (result == null)
                return _responseHandler.NotFound<ReturnRestaurantQuery>("Restaurant not found");

            var mapped = _mapper.Map<ReturnRestaurantQuery>(result);

            return _responseHandler.Success(mapped);
        }

        public async Task<Response<Pagination<GetReviewQuery>>> Handle(GetAllReviewsInRestaurantQuery request, CancellationToken cancellationToken)
        {
            var (reviews, count) = await _service.GetReviewAsync(request.specParams);

            if (!reviews.Any())
            {
                return _responseHandler.Success(
                    new Pagination<GetReviewQuery>(
                        request.specParams.PageIndex,
                        request.specParams.PageSize,
                        0,
                        new List<GetReviewQuery>()),
                    "No reviews found");
            }

            var data = _mapper.Map<IReadOnlyList<GetReviewQuery>>(reviews);

            var pagination = new Pagination<GetReviewQuery>(
                request.specParams.PageIndex,
                request.specParams.PageSize,
                count,
                data
            );

            return _responseHandler.Success(pagination, "Reviews fetched successfully");
        }
    }
}

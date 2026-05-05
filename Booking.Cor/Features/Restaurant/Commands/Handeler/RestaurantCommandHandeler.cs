using AutoMapper;
using Booking.Cor.Features.Restaurant.Commands.Model;
using Booking.Cor.Features.Restaurant.Queries.Result;
using Booking.Data.Entities;
using MediatR;
using Restaurant.Cor.Features.Restaurant.Commands.Model;
using Restaurant.Cor.Features.Restaurant.Queries.Result;
using Restaurant.Service;
using Restaurant.Service.Abstracts;

namespace Restaurant.Cor.Features.Restaurant.Commands.Handeler
{
    public class RestaurantCommandHandeler : IRequestHandler<AddRestaurandCommand, Response<ReturnRestaurantQuery>>
                                     , IRequestHandler<UpdateRestaurantCommand, Response<ReturnRestaurantQuery>>
                                     , IRequestHandler<RemoveRestaurantCommand, Response<string>>
                                     , IRequestHandler<AddReviewCommand, Response<GetReviewQuery>>
    {
        private readonly IRestaurantService _service;
        private readonly ResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public RestaurantCommandHandeler(IRestaurantService service, IMapper mapper, ResponseHandler responseHandler
            )
        {
            _service = service;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }
        public async Task<Response<ReturnRestaurantQuery>> Handle(AddRestaurandCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var restaurant = await _service.AddRestaurantAsync(
                    _mapper.Map<Book.Data.Entities.Restaurant>(request)
                );

                var mapped = _mapper.Map<ReturnRestaurantQuery>(restaurant);

                return _responseHandler.Success(mapped, "Restaurant created successfully");
            }
            catch (Exception ex)
            {
                return _responseHandler.Conflict<ReturnRestaurantQuery>(ex.Message);
            }
        }
        public async Task<Response<ReturnRestaurantQuery>> Handle(
            UpdateRestaurantCommand request,
            CancellationToken cancellationToken)
        {

            // 🔹 Call service
            var result = await _service.UpdateRestaurantAsync(
                _mapper.Map<Book.Data.Entities.Restaurant>(request)
            );

            // 🔹 Not Found
            if (result == null)
                return _responseHandler.NotFound<ReturnRestaurantQuery>("Restaurant not found");

            // 🔹 Success
            var mapped = _mapper.Map<ReturnRestaurantQuery>(result);

            return _responseHandler.Success(mapped, "Restaurant updated successfully");
        }
        public async Task<Response<string>> Handle(RemoveRestaurantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteRestaurantAsync();

                return _responseHandler.Deleted<string>();
            }
            catch (KeyNotFoundException ex)
            {
                return _responseHandler.NotFound<string>(ex.Message);
            }
        }

        public async Task<Response<GetReviewQuery>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _service.CreateReviewAsync(_mapper.Map<Review>(request));
            var mapped = _mapper.Map<GetReviewQuery>(review);
            return _responseHandler.Success(mapped, "Review Added Successfully");

        }
    }
}

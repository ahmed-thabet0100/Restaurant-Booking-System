using Booking.Cor.Features.Restaurant.Commands.Model;
using Booking.Cor.Features.Restaurant.Queries.Model;
using Booking.Cor.Features.Restaurant.Queries.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Cor.Bases;
using Restaurant.Cor.Features.Restaurant.Commands.Model;
using Restaurant.Cor.Features.Restaurant.Queries.Model;
using Restaurant.Cor.Features.Restaurant.Queries.Result;
using Restaurant.Infra.Specifications.RestaurantSpecifications;
using Restaurant.Service;


namespace Restaurant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Restaurant : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ResponseHandler responseHandler;

        public Restaurant(IMediator mediator, ResponseHandler responseHandler)
        {
            _mediator = mediator;
            this.responseHandler = responseHandler;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Service.Response<Pagination<ReturnRestaurantQuery>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllRestaurant([FromQuery] RestaurantSpecParams command)
        {
            var response = await _mediator.Send(new GetAllRestaurantListQuery(command));
            return NewResult(response);
        }
        [HttpGet("{Id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Service.Response<ReturnRestaurantQuery>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRestaurant(int Id)
        {
            var result = await _mediator.Send(new GetRestaurantByIdQuery(Id));

            return NewResult(result);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType(typeof(Service.Response<ReturnRestaurantQuery>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateRestaurant(AddRestaurandCommand restaurant)
        {
            var result = await _mediator.Send(restaurant);
            return NewResult(result);
        }

        [HttpPatch("Update")]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType(typeof(Service.Response<ReturnRestaurantQuery>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateRestaurant(UpdateRestaurantCommand restaurant)
        {
            var result = await _mediator.Send(restaurant);
            return NewResult(result);
        }

        [HttpDelete]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteReataurant()
        {
            var result = await _mediator.Send(new RemoveRestaurantCommand());
            return NewResult(result);
        }

        [HttpPost("add-review")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(typeof(Response<GetReviewQuery>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddReview([FromBody] AddReviewCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("get-reviews")]
        [ProducesResponseType(typeof(Response<Pagination<GetReviewQuery>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReviews([FromQuery] GetAllReviewsInRestaurantQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}

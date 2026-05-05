using Booking.Cor.Features.Reservation.Commands.Model;
using Booking.Cor.Features.Reservation.Queries.Model;
using Booking.Cor.Features.Reservation.Queries.Result;
using Booking.Infra.Specifications.ReservationSpecification;
using Booking.Infra.Specifications.ReservationSpecification.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Api.Controllers;
using Restaurant.Cor.Bases;
using Restaurant.Service;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ResponseHandler responseHandler;

        public ReservationController(IMediator mediator, ResponseHandler responseHandler)
        {
            _mediator = mediator;
            this.responseHandler = responseHandler;
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("Staff/{reservationId}")]
        [ProducesResponseType(typeof(Response<GetReservationForStaff>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetReservationForStaff(
            int reservationId)
        {
            var result = await _mediator.Send(
                new GetReservationForStaffQuery(reservationId));

            return NewResult(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("User/{reservationId}")]
        [ProducesResponseType(typeof(Response<GetReservationForUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetReservationForUser(
            int reservationId)
        {
            var result = await _mediator.Send(
                new GetReservationForUserQuery(reservationId));

            return NewResult(result);
        }

        [HttpGet("Staff/GetAllReservation")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(typeof(Response<Pagination<GetReservationForStaff>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllReservationForStaff([FromQuery] ReservationSpecParamsStaff specParams)
        {
            var result = await _mediator.Send(new GetAllReservationForStaffQuery(specParams));

            return NewResult(result);
        }


        [HttpGet("User/GetAllReservation")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(typeof(Response<Pagination<GetReservationForUser>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllReservationForUser([FromQuery] ReservationSpecParamsUser specParams)
        {
            var result = await _mediator.Send(new GetAllReservationForUserQuery(specParams));

            return NewResult(result);
        }

        [HttpPost("Create")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(typeof(Response<CreateReservationCommand>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);
        }

        [HttpPut("Approve")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(typeof(Response<GetReservationForStaff>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApproveReservation([FromBody] ApproveReservationCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);
        }

        [HttpPut("Reject")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RejectReservation([FromBody] RejectReservationCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);
        }

        [HttpPut("Cancel")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelReservation([FromBody] CancelReservationCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);
        }

        [HttpPut("Update-Time")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(typeof(Response<GetReservationForUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateReservationTime([FromBody] UpdateTimeCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);
        }

        [HttpPut("Complete")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CompleteReservation([FromBody] CompeleteReservationCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);
        }
    }
}

using Booking.Cor.Features.Account.Command.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Cor.Features.Account.Command.Model;
using Restaurant.Data.Dtos;
using Restaurant.Service;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ResponseHandler responseHandler;

        public AccountController(IMediator mediator, ResponseHandler responseHandler)
        {
            _mediator = mediator;
            this.responseHandler = responseHandler;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);
        }

        [HttpPost("ResentOtp")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyEmail(ResentOtpCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);
        }

        [HttpPost("verify-email")]
        [ProducesResponseType(typeof(Response<AuthResponseDto>), 200)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(Response<AuthResponseDto>), 200)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LogInCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);
        }

        [HttpPost("ForgetPassword")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);

        }

        [HttpPost("ResetPassword")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);
        }
        [Authorize]
        [HttpPost("ChangePassword")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);
        }

        [HttpPost("InviteAdmin")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> InviteAdmin(InviteAdminCommand command)
        {
            var result = await _mediator.Send(command);

            return (ActionResult)NewResult(result);

        }

        [AllowAnonymous]
        [HttpPost("register-admin")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Register(RegisterAdminCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(Response<AuthResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<AuthResponseDto>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);

            return NewResult(result);
        }

    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Cor.Bases;
using Restaurant.Cor.Features.Table.Commands.Model;
using Restaurant.Cor.Features.Table.Queries.Model;
using Restaurant.Cor.Features.Table.Queries.Result;
using Restaurant.Infra.Specifications.TableSpecifications;
using Restaurant.Service;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : BaseController
    {
        private readonly IMediator _mediator;

        public TableController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // =========================

        /// <summary>
        /// Get all tables in current user's restaurant (with pagination)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Response<Pagination<GetTableDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetAllTables([FromQuery] TableSpecParams query)
        {
            var response = await _mediator.Send(new GetAllTableInRestaurant(query));
            return NewResult(response);
        }

        // =========================

        /// <summary>
        /// Get specific table by table number
        /// </summary>
        [HttpGet("{tableNum}")]
        [ProducesResponseType(typeof(Response<GetTableDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Staff")]

        public async Task<IActionResult> GetTable(int tableNum)
        {
            var response = await _mediator.Send(new GetTableInRestaurant(tableNum));
            return NewResult(response);
        }

        // =========================

        /// <summary>
        /// Add new table to current user's restaurant
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Response<GetTableDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status409Conflict)]
        [Authorize(Roles = "Staff")]

        public async Task<IActionResult> AddTable([FromBody] AddTableCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet("available")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(typeof(Service.Response<Pagination<GetTableDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Service.Response<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAvailableTables([FromQuery] GetAvailableTables query)
        {
            var result = await _mediator.Send(query);

            return NewResult(result);
        }
        // =========================

        /// <summary>
        /// Update existing table
        /// </summary>
        [HttpPatch]
        [ProducesResponseType(typeof(Response<GetTableDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Staff")]

        public async Task<IActionResult> UpdateTable([FromBody] UpdateTableCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        // =========================

        /// <summary>
        /// Delete table by table number
        /// </summary>
        [HttpDelete("{tableNum}")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Staff")]

        public async Task<IActionResult> DeleteTable(int tableNum)
        {
            var response = await _mediator.Send(new DeleteTableCommand(tableNum));
            return NewResult(response);
        }
    }
}

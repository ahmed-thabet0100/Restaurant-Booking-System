using Microsoft.AspNetCore.Mvc;
using Restaurant.Service;

namespace Restaurant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IActionResult NewResult<T>(Response<T> response)
        {
            return (int)response.StatusCode switch
            {
                200 => Ok(response),
                400 => BadRequest(response),
                401 => Unauthorized(response),
                404 => NotFound(response),
                _ => StatusCode(500, response)
            };
        }
    }
}

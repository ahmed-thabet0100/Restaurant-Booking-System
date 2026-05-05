using Microsoft.AspNetCore.Http;
using Restaurant.Service.Abstracts;
using System.Security.Claims;

namespace Restaurant.Service.Implementation
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public CurrentUserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string UserId =>
            _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string Role =>
            _httpContext.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

        public int? RestaurantId =>
            int.TryParse(_httpContext.HttpContext.User.FindFirst("RestaurantId")?.Value, out var id)
            ? id : null;
        public string Email =>
    _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public string UserName =>
    _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

    }
}

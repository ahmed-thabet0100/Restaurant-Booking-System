using Microsoft.AspNetCore.Identity;
using Restaurant.Data.Entities.Identity;
using System.Security.Claims;

namespace Restaurant.Service.Abstracts
{
    public interface ITokernService
    {
        Task<string> CreateToken(AppUser appUser, UserManager<AppUser> user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string HashToken(string token);
    }
}

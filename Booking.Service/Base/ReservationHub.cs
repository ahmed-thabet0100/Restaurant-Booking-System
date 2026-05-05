using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Booking.Service.Base
{
    [Authorize(Roles = "Staff,Manager")]
    public class ReservationHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var restaurantId = Context.User?.FindFirst("RestaurantId")?.Value;
            var role = Context.User?.FindFirst(ClaimTypes.Role)?.Value;

            if (!string.IsNullOrEmpty(restaurantId))
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"restaurant-{restaurantId}"
                );
            }

            if (role == "Staff")
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"staff-{restaurantId}"
                );
            }

            if (role == "Manager")
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"manager-{restaurantId}"
                );
            }
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"user-{userId}");
            }

            await base.OnConnectedAsync();
        }
    }
}

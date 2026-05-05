using Restaurant.Data.Entities;

namespace Restaurant.Infra.Abstracts
{
    public interface IInvetationRepo
    {
        Task AddAsync(RestaurantAdminInvitation invitation);
        Task UpdateAsync(RestaurantAdminInvitation invitation);
        Task<RestaurantAdminInvitation> GetByCodeAsync(string code);
        Task<RestaurantAdminInvitation> GetActiveInvitationAsync(string email, int restaurantId);
    }
}

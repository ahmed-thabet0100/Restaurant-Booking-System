using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Entities;
using Restaurant.Infra.Abstracts;
using Restaurant.Infra.Data;

namespace Restaurant.Infra.Repostries
{
    public class InvetationRepo : IInvetationRepo
    {
        private readonly AppDbContext _context;

        public InvetationRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(RestaurantAdminInvitation invitation)
        {
            try
            {
                await _context.AddAsync(invitation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<RestaurantAdminInvitation> GetByCodeAsync(string code)
        {
            return await _context.Set<RestaurantAdminInvitation>().FirstOrDefaultAsync(x =>
            x.Code == code);
        }

        public async Task UpdateAsync(RestaurantAdminInvitation invitation)
        {
            try
            {
                _context.Update(invitation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public async Task<RestaurantAdminInvitation> GetActiveInvitationAsync(string email, int restaurantId)
        {
            return await _context.Set<RestaurantAdminInvitation>()
                .FirstOrDefaultAsync(x =>
                    x.Email == email &&
                    x.RestaurantId == restaurantId &&
                    !x.IsUsed &&
                    x.ExpiryDate > DateTime.UtcNow
                );
        }

    }
}

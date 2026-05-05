using Book.Data.Entities;
using Restaurant.Infra.Abstracts;
using Restaurant.Infra.Data;
using Restaurant.Infra.InfraBases;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Infra.Repostries
{
    public class RestaurantRepo : GenericRepo<Book.Data.Entities.Restaurant>, IRestaurantRepo
    {
        public RestaurantRepo(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsNameExistAsync(string name)
        {
            return await _dbContext.Restaurants
    .AnyAsync(r => r.Name.ToLower() == name.ToLower());
        }
    }
}

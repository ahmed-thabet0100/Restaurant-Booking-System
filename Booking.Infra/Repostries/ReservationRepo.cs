using Booking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Dtos;
using Restaurant.Infra.Abstracts;
using Restaurant.Infra.Data;
using Table = Book.Data.Entities.Table;

namespace Booking.Infra.Repostries
{
    public class ReservationRepo : IReservationRepo
    {
        private readonly AppDbContext _dbContext;

        public ReservationRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> IsTableAvilable(int RestaurantId, int TableNum, DateTime start, DateTime end)
        {
            var hasConflict = await _dbContext.Set<Reservation>()
                   .AnyAsync(r =>
                       r.RestaurantId == RestaurantId &&
                       r.TableNumber == TableNum &&
                       r.Status == ReservationStatus.Approved &&
                       start < r.EndDateTime &&
                       end > r.StartDateTime
                   );

            return !hasConflict;
        }
        public async Task<List<Table?>> GetAvailableTables(int restaurantId, int guests, DateTime start, DateTime end)
        {
            var tables = await _dbContext.Set<Table>()
                .Where(t =>
                    t.RestaurantId == restaurantId &&
                    t.Capacity >= guests)
                .OrderBy(t => t.Capacity)
                .ToListAsync();

            var availableTables = new List<Table>();

            foreach (var table in tables)
            {
                var isAvailable = await IsTableAvilable(table.RestaurantId, table.TableNumber, start, end);

                if (!isAvailable)
                    continue;

                availableTables.Add(table);
            }

            return availableTables;
        }

    }
}

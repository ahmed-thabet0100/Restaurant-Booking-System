using Book.Data.Entities;
using Booking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Dtos;
using Restaurant.Infra.Abstracts;
using Restaurant.Infra.Data;
using Restaurant.Infra.InfraBases;

namespace Restaurant.Infra.Repostries
{
    public class TableRepo : GenericRepo<Book.Data.Entities.Table>, ITableRepo
    {
        public TableRepo(AppDbContext dbContext) : base(dbContext)
        {
        }


        public async Task<bool> IsTableNumberExist(int TableNum, int restaurantId)
        {
            return await _dbContext.Tables.AnyAsync(t => t.TableNumber == TableNum && t.RestaurantId == restaurantId);
        }
        public async Task<Book.Data.Entities.Table> GetTableAsync(int TableNum, int restaurantId)
        {
            var IsExist = IsTableNumberExist(TableNum, restaurantId);
            if (!IsExist.Result)
                return null;
            var table = await _dbContext.Tables.SingleOrDefaultAsync(t => t.TableNumber == TableNum && t.RestaurantId == restaurantId);
            return (Book.Data.Entities.Table)table;

        }

        public async Task<IEnumerable<Book.Data.Entities.Table>> GetAllTableInRestaurantAsync(int restaurantId)
        {
            return await _dbContext.Tables
                                .Where(t => t.RestaurantId == restaurantId)
                                .ToListAsync();
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
            if (availableTables == null)
                return new List<Table?>();

            return availableTables;
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
    }
}

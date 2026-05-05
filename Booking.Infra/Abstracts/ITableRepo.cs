using Book.Data.Entities;

namespace Restaurant.Infra.Abstracts
{
    public interface ITableRepo
    {
        public Task<bool> IsTableNumberExist(int TableNum, int restaurantId);
        public Task<Table> GetTableAsync(int TableNum, int restaurantId);
        public Task<IEnumerable<Table>> GetAllTableInRestaurantAsync(int restaurantId);
        public Task<bool> IsTableAvilable(int RestaurantId, int TableNum, DateTime start, DateTime end);
        public Task<List<Table?>> GetAvailableTables(int restaurantId, int guests, DateTime start, DateTime end);

    }
}

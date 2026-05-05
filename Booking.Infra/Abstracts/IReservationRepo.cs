using Book.Data.Entities;

namespace Restaurant.Infra.Abstracts
{
    public interface IReservationRepo
    {
        public Task<bool> IsTableAvilable(int RestaurantId, int TableNum, DateTime start, DateTime end);
        public Task<List<Table?>> GetAvailableTables(int restaurantId, int guests, DateTime start, DateTime end);
    }
}

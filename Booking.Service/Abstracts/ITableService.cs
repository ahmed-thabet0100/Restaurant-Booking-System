using Book.Data.Entities;
using Restaurant.Infra.Specifications.TableSpecifications;

namespace Restaurant.Service.Abstracts
{
    public interface ITableService
    {
        Task<Table> AddTableAsync(Table table);

        Task<(IReadOnlyList<Table> Data, int Count)> GetAllTablesInRestaurantAsync(TableSpecParams specParams);

        Task<Table?> GetTableByIdAsync(int tableId);

        Task DeleteTableAsync(int tableId);

        Task<Table>? UpdateTableAsync(Table entity);
        Task<(IReadOnlyList<Table> Data, int Count)> GetAvailableTables(int guests, DateTime start, DateTime end);

    }
}

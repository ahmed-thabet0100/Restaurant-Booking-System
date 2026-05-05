using Book.Data.Entities;
using Restaurant.Infra.Abstracts;
using Restaurant.Infra.Specifications.TableSpecifications;
using Restaurant.Service.Abstracts;
using Talabat.Core.Repo.Contarct;

namespace Restaurant.Service.Implementation
{
    public class TableService : ITableService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly ITableRepo _tableRepo;

        public TableService(IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            ITableRepo tableRepo)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _tableRepo = tableRepo;
        }

        // 🔹 Helper
        public int GetRestaurantId()
        {
            if (!_currentUser.RestaurantId.HasValue)
                throw new UnauthorizedAccessException("User has no restaurant");

            return _currentUser.RestaurantId.Value;
        }

        // =========================

        public async Task<Table> AddTableAsync(Table table)
        {
            var restaurantId = GetRestaurantId();

            var exists = await _unitOfWork.Repository<Table>()
                .GetEntityWithSpecAsync(
                    new OneTableInRestaurantSpecification(table.TableNumber, restaurantId));

            if (exists != null)
                throw new Exception("Table already exists");

            table.RestaurantId = restaurantId;

            await _unitOfWork.Repository<Table>().AddAsync(table);
            await _unitOfWork.CompleteAsync();

            return table;
        }


        public async Task DeleteTableAsync(int tableNum)
        {
            var restaurantId = GetRestaurantId();

            var table = await _unitOfWork.Repository<Table>()
                .GetEntityWithSpecAsync(
                    new OneTableInRestaurantSpecification(tableNum, restaurantId));

            if (table == null)
                throw new KeyNotFoundException("Table not found");

            _unitOfWork.Repository<Table>().Delete(table);
            await _unitOfWork.CompleteAsync();
        }


        public async Task<(IReadOnlyList<Table> Data, int Count)> GetAllTablesInRestaurantAsync(TableSpecParams specParams)
        {
            var restaurantId = GetRestaurantId();

            var tables = await _unitOfWork.Repository<Table>()
                .GetAllEntitiesWithSpecAsync(
                    new AllTablesInRestaurantSpecification(specParams, restaurantId));

            var count = await _unitOfWork.Repository<Table>()
                .CountAsync(
                    new TableForCountSpecification(specParams, restaurantId));

            return (tables, count);
        }



        public async Task<Table> GetTableByIdAsync(int tableId)
        {
            var restaurantId = GetRestaurantId();

            var table = await _unitOfWork.Repository<Table>()
                .GetEntityWithSpecAsync(
                    new OneTableInRestaurantSpecification(tableId, restaurantId));

            if (table == null)
                throw new KeyNotFoundException("Table not found");

            return table;
        }


        public async Task<Table> UpdateTableAsync(Table entity)
        {
            var restaurantId = GetRestaurantId();

            var table = await _unitOfWork.Repository<Table>()
                .GetEntityWithSpecAsync(
                    new OneTableInRestaurantSpecification(entity.TableNumber, restaurantId));

            if (table == null)
                throw new KeyNotFoundException("Table not found");

            table.Capacity = entity.Capacity;

            await _unitOfWork.Repository<Table>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return table;
        }

        public async Task<(IReadOnlyList<Table> Data, int Count)> GetAvailableTables(int guests, DateTime start, DateTime end)
        {
            var tables = await _tableRepo.GetAvailableTables(GetRestaurantId(), guests, start, end);
            return (tables, tables.Count);
        }

    }
}

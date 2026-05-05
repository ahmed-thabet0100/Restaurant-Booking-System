using Restaurant.Infra.Specifications.Bases;

namespace Restaurant.Infra.InfraBases
{
    public interface IGenericRepo<T> where T : class
    {
        Task<T?> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetTableAsNoTracking();

        Task AddAsync(T Entity);
        Task Update(T Entity);
        void Delete(T Entity);
        Task Remove(T Entity);

        #region specification
        // ✅ Advanced methods using Specification Pattern
        Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllEntitiesWithSpecAsync(ISpecification<T> spec);

        //// ✅ Count method (مفيدة في pagination)
        Task<int> CountAsync(ISpecification<T> spec);
        IQueryable<T> GetTable();
        #endregion

    }
}

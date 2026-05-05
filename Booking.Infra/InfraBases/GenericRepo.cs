using Restaurant.Infra.Data;
using Restaurant.Infra.Specifications.Bases;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Infra.InfraBases
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        protected readonly AppDbContext _dbContext;

        public GenericRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }


        #region specification
        public async Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec)
        {
            return await SpecificationEvaluator<T>
                .GetQuery(_dbContext.Set<T>().AsQueryable(), spec)
                .FirstOrDefaultAsync();
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(
                _dbContext.Set<T>().AsQueryable(),
                spec);
        }

        public async Task<IReadOnlyList<T>> GetAllEntitiesWithSpecAsync(ISpecification<T> spec)
        {
            return await SpecificationEvaluator<T>
                .GetQuery(_dbContext.Set<T>().AsQueryable(), spec)
                .ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }


        #endregion
        public async Task AddAsync(T Entity)
        {
            await _dbContext.AddAsync(Entity);

        }

        public async Task Update(T Entity)
        {
            _dbContext.Update(Entity);
        }

        public void Delete(T Entity)
        {
            _dbContext.Remove(Entity);
        }

        public async Task Remove(T Entity)
        {
            _dbContext.Remove(Entity);
        }


        IQueryable<T> IGenericRepo<T>.GetTableAsNoTracking()
        {
            return _dbContext.Set<T>().AsNoTracking().AsQueryable();
        }

        IQueryable<T> IGenericRepo<T>.GetTable()
        {
            return _dbContext.Set<T>().AsQueryable();
        }
    }
}

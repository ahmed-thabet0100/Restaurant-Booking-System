using Restaurant.Infra.Data;
using Restaurant.Infra.InfraBases;
using Talabat.Core.Repo.Contarct;

namespace Talabat.Repo.Repo_Impelemnt
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private Dictionary<string, object> _repositories;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<string, object>();
        }
        public IGenericRepo<TEntity> Repository<TEntity>() where TEntity : class
        {
            var key = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(key))
            {
                var repository = new GenericRepo<TEntity>(_dbContext);
                _repositories.Add(key, repository);
            }

            return _repositories[key] as IGenericRepo<TEntity>;
        }


        public async Task<int> CompleteAsync() =>
            await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync() =>
            await _dbContext.DisposeAsync();

    }
}

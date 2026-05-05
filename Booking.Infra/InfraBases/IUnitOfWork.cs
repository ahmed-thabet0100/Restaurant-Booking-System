using Restaurant.Infra.InfraBases;

namespace Talabat.Core.Repo.Contarct
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepo<TEntity> Repository<TEntity>() where TEntity : class;

        Task<int> CompleteAsync();
    }
}

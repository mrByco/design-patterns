using System.ComponentModel;

namespace App.DataStore;

public interface IEntityStore<TEntity>: IQueryable<TEntity>, IAsyncEnumerable<TEntity>, IEnumerable<TEntity>, IListSource
{
    void Add(TEntity entity);
    void Delete(TEntity entity);
    Task SaveChangesAsync();
}

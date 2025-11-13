using System.ComponentModel;

namespace App.DataStore;

public interface IEntityStore<TEntity>: IQueryable<TEntity>
{
    void Add(TEntity entity);
    void Delete(TEntity entity);
    Task SaveChangesAsync();
}

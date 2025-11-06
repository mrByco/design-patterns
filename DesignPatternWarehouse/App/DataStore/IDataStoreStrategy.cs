namespace App.DataStore;

public interface IDataStoreStrategy
{
    IEntityStore<TEntity> GetEntityStore<TEntity>();
}

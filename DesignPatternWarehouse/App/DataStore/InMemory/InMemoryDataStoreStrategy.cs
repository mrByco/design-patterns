namespace App.DataStore.InMemory;

public class InMemoryDataStoreStrategy : IDataStoreStrategy
{
    public IEntityStore<TEntity> GetEntityStore<TEntity>()
    {
        return new MemoryEntityStore<TEntity>([]);
    }
}

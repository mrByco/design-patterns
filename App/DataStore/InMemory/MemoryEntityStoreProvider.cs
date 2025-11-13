namespace App.DataStore.InMemory;

internal class MemoryEntityStoreProvider : IDataStoreStrategy
{
    public IEntityStore<TEntity> GetEntityStore<TEntity>()
    {
        return new MemoryEntityStore<TEntity>();
    }
}

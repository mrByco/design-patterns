namespace App.DataStore.JsonFile;

internal class JsonDataStoreStrategy : IDataStoreStrategy
{
    public IEntityStore<TEntity> GetEntityStore<TEntity>()
    {
        var filename = typeof(TEntity).Name + ".json";
        return new JsonFileEntityStore<TEntity>(filename);
    }
}

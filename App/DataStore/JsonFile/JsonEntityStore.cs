namespace App.DataStore.JsonFile;
public class JsonEntityStoreProvider : IDataStoreStrategy
{
    public IEntityStore<TEntity> GetEntityStore<TEntity>()
    {
        return new JsonFileEntityStore<TEntity>("data.json");
    }
}
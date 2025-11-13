using App.DataStore;
using App.Models;
using DI;

namespace App.Repositories;

public class WarehosueRepository : IWarehouseRepository
{
    private IEntityStore<Warehouse> items;

    [DI]
    public IDataStoreStrategy Storage
    {
        init => items = value.GetEntityStore<Warehouse>();
    }

    public List<int> GetWarehouseIds()
    {
        return items.Select(x => x.Id).ToList();
    }

    public async Task AddWarehouse(Warehouse warehouse)
    {
        items.Add(warehouse);
        await items.SaveChangesAsync();
    }


    public async Task DeleteWarehouse(int id)
    {
        var instance = items.FirstOrDefault(x => x.Id == id);
        items.Delete(instance);
        await items.SaveChangesAsync();
    }

    public async Task Update(int id, Warehouse warehosue)
    {
        await DeleteWarehouse(id);
        await AddWarehouse(warehosue);
        await items.SaveChangesAsync();
    }

    public async Task<Warehouse> GetWarehouse(int id)
    {
        return items.FirstOrDefault(x => x.Id == id);
    }
}

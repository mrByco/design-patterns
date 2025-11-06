using App.Models;

namespace App.Repositories;

public interface IWarehouseRepository
{
    public List<int> GetWarehouseIds();
    Task AddWarehouse(Warehouse warehouse);
    Task DeleteWarehouse(int id);
    Task Update(int id, Warehouse warehosue);
}

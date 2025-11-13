
using App.Repositories;
using DI;

namespace App.Commands;

public class WarehouseListCommand : ICommand
{
    [DI]
    public IWarehouseRepository _warehouseRepostiry { private get; init; }

    public async Task Execute()
    {
        var ids = _warehouseRepostiry.GetWarehouseIds();

        Console.WriteLine("Warehouses: ");
        foreach (var id in ids)
        {
            Console.WriteLine($"  - {id.ToString()}");
        }
    }
}

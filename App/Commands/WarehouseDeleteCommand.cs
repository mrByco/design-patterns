using App.Repositories;
using DI;

namespace App.Commands;

public class WarehouseDeleteCommand : ICommand
{
    [DI]
    public IWarehouseRepository _warehouseRepostiry { private get; init; }

    public int _id;

    public WarehouseDeleteCommand(int id)
    {

    }

    public async Task Execute()
    {
        var warehouse = await _warehouseRepostiry.GetWarehouse(_id);
        if (warehouse == null) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Warehouse not found!");
            Console.ResetColor();
            return;
        }

        await _warehouseRepostiry.DeleteWarehouse(_id);
    }
}

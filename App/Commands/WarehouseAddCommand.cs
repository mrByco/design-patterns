using App.Models;
using App.Repositories;
using DI;

namespace App.Commands;

public class WarehouseAddCommand : ICommand
{
    [DI]
    public IWarehouseRepository _warehouseRepostiry { private get; init; }

    public string name { get;set; }

    public WarehouseAddCommand(string name)
    {
        this.name = name;
    }

    
    public async Task Execute()
    {
        var warehouse = new Warehouse()
        {
            Name = this.name
        };

        await _warehouseRepostiry.AddWarehouse(warehouse);
    }

}

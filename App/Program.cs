// See https://aka.ms/new-console-template for more information
using App;
using App.Commands;
using App.DataStore;
using App.DataStore.InMemory;
using App.DataStore.JsonFile;
using App.Misc;
using App.Models;
using App.Repositories;
using DI.Core;
using System.Reflection;




var container = new DIContainer();
#if DEBUG
container.Register<IDataStoreStrategy, MemoryEntityStoreProvider>();
#else
container.Register<IDataStoreStrategy, JsonEntityStoreProvider>();
#endif
container.Register<IWarehouseRepository, WarehosueRepository>();
container.Register<ICommandRepository, CommandRepository>();

var commandContainer = CommandContainer.GetInstance();
commandContainer.AddCommandsFromAssembly(Assembly.GetExecutingAssembly());

while (true)
{
    try
    {
        Console.Write("// ");
        var line = Console.ReadLine();
        var command = commandContainer.GetCommand(line, container, new CommandFactory());
        if (command == null)
        {
            continue;
        }

        await command.Execute();
        var storedCmd = new StoredCommand()
        {
            Command = command,
            ExecutionDate = DateTime.Now,
        };
        await container.Resolve<ICommandRepository>()
            .StoreCommand(storedCmd);
    }
    catch (Exception ex) {
        Console.WriteLine(ex.ToString());
    }
}


/* 
 
Design patternek

1. Command: Commands
2. Strategy: DataStores
3. Singleton: CommandContainer

*/
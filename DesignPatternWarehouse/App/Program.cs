// See https://aka.ms/new-console-template for more information
using App;
using App.DataStore;
using App.DataStore.InMemory;
using App.DataStore.JsonFile;
using App.Misc;
using App.Repositories;
using DI.Core;
using System.Reflection;

Console.WriteLine("Hello");



var container = new DIContainer();
//container.Register<IDataStoreStrategy, InMemoryDataStoreStrategy>();
container.Register<IDataStoreStrategy, JsonDataStoreStrategy>();
container.Register<IWarehouseRepository, WarehosueRepository>();

var commandContainer = CommandContainer.GetInstance();
commandContainer.AddCommandsFromAssembly(Assembly.GetExecutingAssembly());

while (true)
{
    try
    {
        Console.Write("€ ");
        var line = Console.ReadLine();
        var command = commandContainer.GetCommand(line, container, new CommandFactory());
        if (command == null)
        {
            continue;
        }

        var arguments = line.Split(' ').ToList();
        arguments.RemoveAt(0);

        await command.Execute();
    }
    catch (Exception ex) {
        Console.WriteLine(ex.ToString());
    }
}


/* 
 
Design patternek

1. Singleton: CommandContainer
2. Strategy: DataStores
3. Command: Commands

*/
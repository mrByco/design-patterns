
using App.DataStore;
using App.Models;
using DI;

namespace App.Repositories;

public class CommandRepository : ICommandRepository
{
    private IEntityStore<StoredCommand> commands;

    [DI]
    public IDataStoreStrategy Storage
    {
        init => commands = value.GetEntityStore<StoredCommand>();
    }

    public List<StoredCommand> GetExecutedCommands()
    {
        return commands.ToList();
    }

    public async Task StoreCommand(StoredCommand command)
    {
        commands.Add(command);
        await commands.SaveChangesAsync();
    }
}

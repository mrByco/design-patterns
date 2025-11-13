using App.Models;

namespace App.Repositories;

public interface ICommandRepository
{
    public List<StoredCommand> GetExecutedCommands();
    public Task StoreCommand(StoredCommand command);
}

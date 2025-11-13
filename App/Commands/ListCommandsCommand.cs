
using App.Repositories;
using DI;

namespace App.Commands;

public class ListCommandsCommand : ICommand
{
    [DI]
    public ICommandRepository _commandRepository { private get; init; }
    public Task Execute()
    {
        var commands = _commandRepository.GetExecutedCommands();

        foreach (var command in commands) {
            Console.WriteLine($"{command.Command.GetType().Name.ToLower()} { command.ExecutionDate.ToString("yyyy-MM-dd hh:mm") }");
        }

        return Task.CompletedTask;
    }
}

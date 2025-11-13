
namespace App.Commands;

public class HelpCommand : ICommand
{

    public Task Execute()
    {
        var commandContainer = CommandContainer.GetInstance();

        var commands = commandContainer.GetAllCommands();

        Console.WriteLine("\n\nAvailable commands:");
        foreach (var command in commands)
        {
            var ctors = command.Value.GetConstructors();
            foreach (var ctor in ctors)
            {
                var parameters = ctor.GetParameters();
                var parametersForamtted = string.Join(" ", parameters.Select(x => $"{x.Name}"));
                Console.WriteLine($"  - {command.Key} {parametersForamtted}");
            }
        }
        return Task.CompletedTask;
    }

    public void SetArguments(params string[] args)
    {
    }
}

using App.Commands;
using App.Misc;
using DI;
using System.Reflection;

namespace App;

public class CommandContainer
{
    private static CommandContainer instance;

    private Dictionary<string, Type> commandTypes = new Dictionary<string, Type>();

    private CommandContainer() { }

    public void AddCommandsFromAssembly(Assembly assembly)
    {
        var commands = assembly
            .GetTypes()
            .Where(x => x.Name.EndsWith("Command") && typeof(ICommand).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface);

        foreach (var command in commands)
        {
            commandTypes.Add(command.Name.Replace("Command", "").ToLower(), command);
        }
    }

    public static CommandContainer GetInstance()
    {
        if (instance == null)
        {
            instance = new CommandContainer();
        }

        return instance;
    }

    public ICommand GetCommand(string command, ICustomServiceProvider serviceProvider, ICommandFactory commandFactory)
    {
        var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            throw new ArgumentException("Command cannot be empty.");

        var commandName = parts[0].ToLower();
        if (!commandTypes.TryGetValue(commandName, out var type))
            throw new ArgumentException($"No command found for \"{commandName}\"");


        var commandInstance = commandFactory.CreateCommand(type, serviceProvider, parts);

        return commandInstance;
    }
    public Dictionary<string, Type> GetAllCommands()
    {
        return this.commandTypes;
    }

#if DEBUG
    public static void ResetForTests()
    {
        instance = null;
    }
#endif
}

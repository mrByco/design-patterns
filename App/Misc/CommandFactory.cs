using App.Commands;
using DI;

namespace App.Misc;

// Lehet hogy nem igazi factory de így tesztelhető és ez a legjobb név amit találtam
public class CommandFactory : ICommandFactory
{
    public ICommand CreateCommand(Type type, ICustomServiceProvider serviceProvider, params string[] commandParams)
    {

        var ctor = type.GetConstructors()
            .OrderByDescending(c => c.GetParameters().Length)
            .FirstOrDefault();

        if (ctor == null)
            throw new InvalidOperationException($"No public constructor found for {type.Name}");

        var ctorParams = ctor.GetParameters();
        var args = new object?[ctorParams.Length];

        for (int i = 0; i < ctorParams.Length; i++)
        {
            var param = ctorParams[i];
            var valueIndex = i + 1;

            if (valueIndex < commandParams.Length)
            {
                args[i] = Convert.ChangeType(commandParams[valueIndex], param.ParameterType);
            }
            else
            {
                args[i] = serviceProvider.Resolve(param.ParameterType);
            }
        }

        var commandInstance = (ICommand)ctor.Invoke(args);
        commandInstance.InjectDependencies(serviceProvider);

        return commandInstance;
    }

    
}

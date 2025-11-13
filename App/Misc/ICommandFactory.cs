using App.Commands;
using DI;

namespace App.Misc;

public interface ICommandFactory
{
    ICommand CreateCommand(Type commandType, ICustomServiceProvider serviceProvider, params string[] args);
}

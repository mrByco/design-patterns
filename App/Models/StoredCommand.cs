

using App.Commands;

namespace App.Models;

public class StoredCommand
{
    public ICommand Command { get; set; }
    public DateTime ExecutionDate { get; set; }
}

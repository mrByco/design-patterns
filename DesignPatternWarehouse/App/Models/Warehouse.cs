namespace App.Models;

public class Warehouse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Storage> Storages { get; set; }
}

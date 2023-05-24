namespace Client.Models;

public class FoundItem : Item
{
    public DateTime FoundAt { get; set; }
    public Employee FoundBy { get; set; }
}
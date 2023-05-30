namespace Client.Models;

public class ClaimedItem : Item
{
    public DateTime ClaimedAt { get; set; }
    public Employee ClaimedBy { get; set; }
}
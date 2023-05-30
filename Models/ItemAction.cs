namespace Client.Models;

public class ItemAction
{
    public Guid Id { get; set; }
    public DateTime Time { get; set; }
    public Guid ItemId { get; set; }
    public Guid ActionId { get; set; }
}
namespace Client.Models;

public class RequestFound
{
    public Guid RequestId { get; set; }
    public Item RequestItem { get; set; }
    public Employee RequestBy { get; set; }
    public DateTime RequestAt { get; set; }
    public string Status { get; set; }
}
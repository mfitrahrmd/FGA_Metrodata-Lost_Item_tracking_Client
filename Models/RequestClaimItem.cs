namespace Client.Models;

public class RequestClaimItem
{
    public Guid RequestId { get; set; }
    public Item RequestItem { get; set; }
    public Employee RequestBy { get; set; }
    public DateTime RequestAt { get; set; }
    public string Status { get; set; }
}
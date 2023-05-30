namespace Client.Models;

public class RequestFound
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile File { get; set; }
}
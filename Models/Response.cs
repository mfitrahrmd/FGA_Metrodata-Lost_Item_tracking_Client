namespace Client.Models;

public class Response<TData>
{
    public bool IsSucceeded { get; set; }
    public int Code { get; set; }
    public TData Data { get; set; }
}
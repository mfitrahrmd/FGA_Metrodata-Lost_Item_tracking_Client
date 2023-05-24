namespace Client.Models;

public enum Gender
{
    F,
    M
}

public class Employee
{
    public Guid Id { get; set; }
    public string Nik { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Guid DepartmentId { get; set; }

    public string FullName
    {
        get => $"{FirstName} {LastName}";
    }
}
public class LoginRequest
{
    public string Nik { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string AccessToken { get; set; }
}

public class RegisterRequest
{
    public string Nik { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid DepartmentId { get; set; }
    public string Password { get; set; }
}

public class RegisterResponse
{
    public string Nik { get; set; }
}
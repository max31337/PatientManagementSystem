
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } // Store hashed password in production
    public UserRole Role { get; set; } // Role property
}

public enum UserRole
{
    Admin,
    Doctor,
    Staff
}


using System.ComponentModel.DataAnnotations;

namespace PatientManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Admin,
        Doctor
    }

}


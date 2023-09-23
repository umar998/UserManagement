namespace UserManagement.Common.Schema.Models
{
    public class LoginResponse
    {
        public User User { get; set; } // Use the User class instead of individual properties
        public Roles Role { get; set; } // Use the Roles class instead of string
        public string TokenString { get; set; }
    }
}

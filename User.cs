public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string State { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int LoginAttempts { get; set; }
    public DateTime? LockOutEnd { get; set; }
}
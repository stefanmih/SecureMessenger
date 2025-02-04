namespace SecureMessenger.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PublicIp { get; set; }

    public string PasswordHash { get; set; }
}

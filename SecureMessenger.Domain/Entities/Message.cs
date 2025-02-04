namespace SecureMessenger.Domain.Entities;

public class Message
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; } 
    public string EncryptedContent { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}

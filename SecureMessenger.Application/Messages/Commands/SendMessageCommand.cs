using MediatR;

namespace SecureMessenger.Application.Messages.Commands;

public class SendMessageCommand : IRequest<int>
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string PlainContent { get; set; }
    public string EncryptionKey { get; set; }
}

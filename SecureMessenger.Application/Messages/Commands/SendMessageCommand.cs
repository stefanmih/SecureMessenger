using MediatR;

namespace SecureMessenger.Application.Messages.Commands;

public class SendMessageCommand : IRequest<int>
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public required string PlainContent { get; set; }
    public required string EncryptionKey { get; set; }
}

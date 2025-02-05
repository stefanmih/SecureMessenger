using MediatR;

namespace SecureMessenger.Application.Messages.Commands;

public class CreateMessageCommand : IRequest<int>
{
    public required string Sender { get; set; }
    public required string Receiver { get; set; }
    public required string Content { get; set; }
}

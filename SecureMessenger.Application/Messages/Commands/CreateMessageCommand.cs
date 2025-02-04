using MediatR;

namespace SecureMessenger.Application.Messages.Commands;

public class CreateMessageCommand : IRequest<int>
{
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public string Content { get; set; }
}

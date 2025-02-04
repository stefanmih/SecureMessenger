using MediatR;
using SecureMessenger.Domain.Entities;

namespace SecureMessenger.Application.Messages.Queries;

public class GetMessagesQuery : IRequest<List<Message>>
{
    public int ReceiverId { get; set; }
}

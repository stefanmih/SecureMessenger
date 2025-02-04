using MediatR;
using Microsoft.EntityFrameworkCore;
using SecureMessenger.Domain.Entities;
using SecureMessenger.Infrastructure.Persistence;

namespace SecureMessenger.Application.Messages.Queries;

public class GetMessagesHandler : IRequestHandler<GetMessagesQuery, List<Message>>
{
    private readonly MessengerDbContext _context;

    public GetMessagesHandler(MessengerDbContext context)
    {
        _context = context;
    }

    public async Task<List<Message>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Messages
            .Where(m => m.ReceiverId == request.ReceiverId)
            .ToListAsync(cancellationToken);
    }
}

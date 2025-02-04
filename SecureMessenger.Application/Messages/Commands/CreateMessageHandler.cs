using MediatR;
using SecureMessenger.Domain.Entities;
using SecureMessenger.Infrastructure.Persistence;
using SecureMessenger.Infrastructure.Services;

namespace SecureMessenger.Application.Messages.Commands;

public class CreateMessageHandler : IRequestHandler<SendMessageCommand, int>
{
    private readonly MessengerDbContext _context;

    public CreateMessageHandler(MessengerDbContext context, AesEncryptionService encryptionService)
    {
        _context = context;
    }

    public async Task<int> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new Message
        {
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            EncryptedContent = request.PlainContent
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);

        return message.Id;
    }
}

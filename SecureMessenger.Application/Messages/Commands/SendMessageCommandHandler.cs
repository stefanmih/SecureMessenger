using MediatR;
using SecureMessenger.Domain.Entities;
using SecureMessenger.Infrastructure.Persistence;
using System.Windows;

namespace SecureMessenger.Application.Messages.Commands
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, int>
    {
        private readonly MessengerDbContext _context;

        public SendMessageCommandHandler(MessengerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"InnerException: {ex.InnerException.Message}");
                }

                MessageBox.Show($"Error: {ex.Message}\n{ex.InnerException?.Message}",
                                               "Database Error",
                                               MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }


    }
}

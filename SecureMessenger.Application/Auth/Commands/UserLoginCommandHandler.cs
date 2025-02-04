using MediatR;
using Microsoft.EntityFrameworkCore;
using SecureMessenger.Infrastructure.Persistence;
using System.Security.Cryptography;
using System.Text;

namespace SecureMessenger.Application.Auth.Commands
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, int?>
    {
        private readonly MessengerDbContext _context;

        public UserLoginCommandHandler(MessengerDbContext context)
        {
            _context = context;
        }

        public async Task<int?> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                return null;

            return user.Id;
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return hash == storedHash;
            }
        }
    }
}

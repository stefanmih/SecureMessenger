using MediatR;
using Microsoft.EntityFrameworkCore;
using SecureMessenger.Domain.Entities;
using SecureMessenger.Infrastructure.Persistence;
using System.Text;

namespace SecureMessenger.Application.Auth.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, int>
    {
        private readonly MessengerDbContext _context;

        public RegisterUserCommandHandler(MessengerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

            if (existingUser != null)
                return -1;

            var user = new User
            {
                Username = request.Username,
                PasswordHash = HashPassword(request.Password),
                PublicIp = request.PublicIp
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return user.Id;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
    }
}

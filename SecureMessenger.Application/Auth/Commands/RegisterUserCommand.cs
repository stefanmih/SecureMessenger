using MediatR;

namespace SecureMessenger.Application.Auth.Commands
{
    public class RegisterUserCommand : IRequest<int>
    {
        public required string Username { get; set; } = null!;
        public required string Password { get; set; } = null!;
        public required string PublicIp { get; set; } = null!;
    }
}

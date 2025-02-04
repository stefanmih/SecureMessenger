using MediatR;

namespace SecureMessenger.Application.Auth.Commands
{
    public class RegisterUserCommand : IRequest<int>
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PublicIp { get; set; } = null!;
    }
}

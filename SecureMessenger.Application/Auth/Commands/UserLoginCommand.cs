using MediatR;

namespace SecureMessenger.Application.Auth.Commands
{
    public class UserLoginCommand : IRequest<int?>
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

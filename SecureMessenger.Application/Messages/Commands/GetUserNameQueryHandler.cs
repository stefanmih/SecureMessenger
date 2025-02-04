using MediatR;
using SecureMessenger.Infrastructure.Persistence;

public class GetUserNameQueryHandler : IRequestHandler<GetUserNameQuery, string>
{
    private readonly MessengerDbContext _context;

    public GetUserNameQueryHandler(MessengerDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(GetUserNameQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        return user.Username;
    }
}

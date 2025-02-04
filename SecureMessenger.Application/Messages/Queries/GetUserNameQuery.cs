using MediatR;

public class GetUserNameQuery : IRequest<string>
{
    public int UserId { get; set; }
}

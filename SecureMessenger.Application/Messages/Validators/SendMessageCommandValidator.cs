using FluentValidation;
using SecureMessenger.Application.Messages.Commands;

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.SenderId).GreaterThan(0).WithMessage("SenderId must be greater than 0");
        RuleFor(x => x.ReceiverId).GreaterThan(0).WithMessage("ReceiverId must be greater than 0");
        RuleFor(x => x.PlainContent).NotEmpty().WithMessage("PlainContent is required");
        RuleFor(x => x.EncryptionKey).NotEmpty().WithMessage("EncryptionKey is required");
    }
}

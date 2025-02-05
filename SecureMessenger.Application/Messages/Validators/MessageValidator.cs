using FluentValidation;
using SecureMessenger.Domain.Entities;

namespace SecureMessenger.Application.Messages.Validators
{
    internal class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(x => x.SenderId).GreaterThan(0).WithMessage("SenderId must be greater than 0");
            RuleFor(x => x.ReceiverId).GreaterThan(0).WithMessage("ReceiverId must be greater than 0");
            RuleFor(x => x.EncryptedContent).NotEmpty().WithMessage("EncryptionContent is required");
        }
    }
}

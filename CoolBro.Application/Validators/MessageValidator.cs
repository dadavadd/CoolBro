using CoolBro.Domain.Entities;
using FluentValidation;

namespace CoolBro.Application.Validators;

public class MessageValidator : AbstractValidator<Message>
{
    public MessageValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(1000)
            .WithMessage("Message content must not exceed 1000 characters");

        RuleFor(x => x.CreatedAt)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow);

        RuleFor(x => x.Response)
            .MaximumLength(2000)
            .When(x => x.Response != null);

        RuleFor(x => x.UserId)
            .NotEmpty()
            .GreaterThan(0);
    }
}

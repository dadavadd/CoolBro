using CoolBro.Domain.Entities.UserEntity;
using FluentValidation;

namespace CoolBro.Application.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(32)
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("Username can only contain letters, numbers and underscores");

        RuleFor(x => x.TelegramId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Role)
            .IsInEnum();
    }
}

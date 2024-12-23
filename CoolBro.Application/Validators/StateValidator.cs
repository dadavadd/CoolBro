using CoolBro.Domain.Entities;
using FluentValidation;

namespace CoolBro.Application.Validators;

public class StateValidator : AbstractValidator<State>
{
    public StateValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.CurrentState)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.StateData)
            .MaximumLength(4000)
            .When(x => x.StateData != null);
    }
}

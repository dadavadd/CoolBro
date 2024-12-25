using CoolBro.Application.Interfaces;
using CoolBro.Application.Validators;
using CoolBro.Domain.Entities;
using CoolBro.Domain.Entities.UserEntity;
using CoolBro.Infrastructure.Data.Interfaces;
using FluentValidation;

namespace CoolBro.Application.Services.SessionServices;

public class SessionService(
    ISessionRepository sessionRepository,
    IValidator<State> stateValidator) : ISessionService
{
    public async Task<State> GetOrCreateSessionAsync(User user)
    {
        var session = await sessionRepository.GetUserSessionByIdAsync(user.Id);

        if (session is null)
        {
            session = new()
            {
                UserId = user.Id,
                User = user,
                CurrentState = "Start"
            };

            var validationResult = await stateValidator.ValidateAsync(session);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await sessionRepository.SetUserSessionAsync(session);
        }

        return session;
    }
}

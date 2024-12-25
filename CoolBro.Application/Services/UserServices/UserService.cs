using CoolBro.Application.Interfaces;
using CoolBro.Domain.Entities;
using CoolBro.Domain.Entities.UserEntity;
using CoolBro.Domain.Enums;
using CoolBro.Infrastructure.Data.Interfaces;
using FluentValidation;

namespace CoolBro.Application.Services.UserServices;

public class UserService(
    IUserRepository userRepository,
    IValidator<User> userValidator) : IUserService
{
    public async Task<User> GetOrCreateUserAsync(long userId, string username)
    {
        var user = await userRepository.GetByTelegramIdAsync(userId);

        if (user is null)
        {
            user = new()
            {
                TelegramId = userId,
                Username = username ?? $"user_{userId}",
                Role = Roles.User,
                Balance = new() { Balance = 0 },

                CreatedAt = DateTime.UtcNow,
                Messages = new List<Message>()
            };

            var validationResult = await userValidator.ValidateAsync(user);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            user = await userRepository.CreateAsync(user);
        }

        return user;
    }
}

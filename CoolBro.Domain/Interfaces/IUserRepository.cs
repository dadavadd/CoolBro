
using CoolBro.Domain.Entities;
using CoolBro.Domain.Enums;

namespace CoolBro.Infrastructure.Data.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByTelegramIdAsync(long telegramId);
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
    Task<IEnumerable<User>> GetUsersByRoleAsync(Roles role);
}
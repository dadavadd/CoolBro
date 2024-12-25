using CoolBro.Domain.Entities.UserEntity;
using CoolBro.Domain.Enums;
using CoolBro.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoolBro.Infrastructure.Data.Repositories;

public class UserRepository(ApplicationDbContext context) : RepositoryBase<User>(context), IUserRepository
{
    public async Task<User?> GetByTelegramIdAsync(long telegramId) =>
        await Query
            .Include(u => u.Session)
            .Include(u => u.Balance)
            .FirstOrDefaultAsync(u => u.TelegramId == telegramId);

    public async Task<User?> GetByIdAsync(int id) =>
        await Query
            .Include(u => u.Session)
            .FirstOrDefaultAsync(u => u.Id == id);


    public async Task<User> CreateAsync(User user) =>
        await InsertAsync(user);


    public async Task<IEnumerable<User>> GetUsersByRoleAsync(Roles role) =>
        await Query.
        Where(u => u.Role == role)
        .ToListAsync();
}
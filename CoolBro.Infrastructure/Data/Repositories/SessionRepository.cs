using CoolBro.Domain.Entities;
using CoolBro.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoolBro.Infrastructure.Data.Repositories;

public class SessionRepository(
    ApplicationDbContext context
    ) : RepositoryBase<State>(context), ISessionRepository
{
    public async Task<State?> GetUserSessionByIdAsync(int userId) =>
        await Query.FirstOrDefaultAsync(u => u.UserId == userId);

    public async Task SetUserSessionAsync(State state) =>
        await UpdateAsync(state);
}

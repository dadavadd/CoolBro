using CoolBro.Domain.Entities;

namespace CoolBro.Infrastructure.Data.Interfaces;

public interface ISessionRepository
{
    Task<State?> GetUserSessionByIdAsync(int userId);
    Task SetUserSessionAsync(State state);
}

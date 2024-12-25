using CoolBro.Domain.Entities;
using CoolBro.Domain.Entities.UserEntity;

namespace CoolBro.Application.Interfaces;

public interface ISessionService
{
    Task<State> GetOrCreateSessionAsync(User user);
}

using CoolBro.Domain.Entities.UserEntity;

namespace CoolBro.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetOrCreateUserAsync(long userId, string username);
    }
}

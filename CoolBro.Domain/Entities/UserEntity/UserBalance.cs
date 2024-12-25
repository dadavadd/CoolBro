

namespace CoolBro.Domain.Entities.UserEntity;

public class UserBalance : BaseEntity
{
    public decimal Balance { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

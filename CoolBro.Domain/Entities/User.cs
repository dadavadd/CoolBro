using CoolBro.Domain.Enums;

namespace CoolBro.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = null!;
    public long TelegramId { get; set; }
    public Roles Role { get; set; } = Roles.User;
    public State Session { get; set; } = null!;

    public ICollection<Message> Messages { get; set; } = null!;
}

namespace CoolBro.Domain.Entities;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

namespace CoolBro.Domain.Entities;

public class Message : BaseEntity
{
    public string Content { get; set; } = null!;
    public bool IsRead { get; set; }
    public string? Response { get; set; }


    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

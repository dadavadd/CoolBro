namespace CoolBro.Domain.Entities;

public class State
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string CurrentState { get; set; } = "Start";
    public string? StateData { get; set; }
}

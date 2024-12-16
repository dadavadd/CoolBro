using CoolBro.Application;
using CoolBro.Application.Services;
using CoolBro.Domain.Entities;
using Telegram.Bot;

namespace CoolBro.UpdateHandlers;

public abstract class UpdateHandlerBase
{
    public ITelegramBotClient Client { get; set; } = null!;
    public User User { get; set; } = null!;
    public SessionManager Session { get; set; } = null!;
    public ExtendedUpdate Update { get; set; } = null!;
}

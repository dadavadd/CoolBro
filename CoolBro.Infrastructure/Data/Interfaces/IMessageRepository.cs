
using CoolBro.Domain.Entities;

namespace CoolBro.Infrastructure.Data.Interfaces;

public interface IMessageRepository
{
    Task<List<Message>?> GetMessagesByTelegramId(long telegramId, int take, int skip);
    Task<List<Message>?> GetMessagesById(int id, int take, int skip);
    Task CreateAsync(Message message);
}

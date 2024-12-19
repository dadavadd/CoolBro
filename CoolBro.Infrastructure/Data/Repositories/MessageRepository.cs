using CoolBro.Domain.Entities;
using CoolBro.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoolBro.Infrastructure.Data.Repositories;

public class MessageRepository(
    ApplicationDbContext context) : RepositoryBase<Message>(context), IMessageRepository
{
    public async Task<Message> CreateMessageAsync(Message message) =>
        await InsertAsync(message);

    public async Task<List<Message>?> GetMessagesById(int id, int take, int skip) =>
        await Query
        .Where(m => m.Id == id)
        .OrderBy(m => m.Id)
        .Skip(skip)
        .Take(take)
        .ToListAsync();

    public async Task<List<Message>?> GetMessagesByTelegramId(long telegramId, int take, int skip) =>
        await Query
        .Include(m => m.User)
        .Where(m => m.User.TelegramId == telegramId)
        .OrderBy(m => m.Id)
        .Skip(skip)
        .Take(take)
        .ToListAsync();

    public async Task<List<Message>> GetAllNoReadMessages(int take, int skip) =>
        await Query
        .Include(m => m.User)
        .Where(m => !m.IsRead)
        .OrderByDescending(m => m.CreatedAt)
        .Skip(skip)
        .Take(take)
        .ToListAsync();

    public async Task UpdateMessageAsync(Message message) =>
        await UpdateAsync(message);

    public async Task DeleteMessageAsync(int id) =>
        await RemoveAsync(await Query.FirstAsync(m => m.Id == id));

}

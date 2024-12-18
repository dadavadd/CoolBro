using CoolBro.Domain.Entities;
using CoolBro.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolBro.Infrastructure.Data.Repositories;

public class MessageRepository(
    ApplicationDbContext context) : RepositoryBase<Message>(context), IMessageRepository
{
    public async Task CreateAsync(Message message) =>
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
        .Where(m => m.User.TelegramId == telegramId)
        .OrderBy(m => m.Id)
        .Skip(skip)
        .Take(take)
        .ToListAsync();

    public async Task DeleteAsync(int id) =>
        await RemoveAsync(await Query.FirstAsync(m => m.Id == id));

}

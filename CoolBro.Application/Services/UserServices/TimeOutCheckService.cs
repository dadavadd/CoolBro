using CoolBro.Application.Interfaces;
using CoolBro.Infrastructure.Data.Interfaces;

namespace CoolBro.Application.Services.UserServices;

public class TimeOutCheckService
    (IMessageRepository messageRepository) : ITimeOutCheckService
{
    public async Task<bool> CheckMessageTimeOutAsync(int entityId, TimeSpan timeOut)
    {
        var lastTicket = await messageRepository.GetMessagesById(entityId, 1, 0);

        return lastTicket != null
            && lastTicket.Any()
            && DateTime.UtcNow - lastTicket[0].CreatedAt > timeOut;
    }
}

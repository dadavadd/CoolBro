namespace CoolBro.Application.Interfaces;

public interface ITimeOutCheckService
{
    Task<bool> CheckMessageTimeOutAsync(int entityId, TimeSpan timeOut);
}

using CoolBro.Domain.Entities;
using CoolBro.Infrastructure.Data.Interfaces;

namespace CoolBro.Application.Services;

public class SessionManager(
    ISessionRepository sessionRepository, 
    State session)
{
    private readonly SessionWrapper _sessionWrapper = new(sessionRepository, session);
    
    public string CurrentState => session.CurrentState;
    public SessionWrapper Wrapper => _sessionWrapper;

    public async Task SetStateAsync(string state)
    {
        session.CurrentState = state;

        await sessionRepository.SetUserSessionAsync(session);
    }

    public void SetData(Dictionary<string, object>? stateData = null)
    {
        if (stateData != null)
        {
            foreach (var (key, value) in stateData)
            {
                _sessionWrapper.Set(key, value);
            }
        }
    }

    public async Task ClearStateAsync()
    {
        _sessionWrapper.Clear();
        await sessionRepository.SetUserSessionAsync(session);
    }
}

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
}

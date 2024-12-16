using CoolBro.Infrastructure.Data;
using CoolBro.Infrastructure.Data.Interfaces;
using CoolBro.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBro.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();

        services.AddDbContext<ApplicationDbContext>(options 
            => options.UseSqlite("Data Source=coolbro.db"));
    }
}

using CoolBro.Application.Interfaces;
using CoolBro.Application.Services.SessionServices;
using CoolBro.Application.Services.UserServices;
using CoolBro.Application.Validators;
using CoolBro.Domain.Entities;
using CoolBro.Domain.Entities.UserEntity;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBro.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        services.AddScoped<ITimeOutCheckService, TimeOutCheckService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IValidator<User>, UserValidator>();
        services.AddScoped<IValidator<Message>, MessageValidator>();
        services.AddScoped<IValidator<State>, StateValidator>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISessionService, SessionService>();
    }
}

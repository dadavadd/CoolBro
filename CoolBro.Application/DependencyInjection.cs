﻿using CoolBro.Application.Interfaces;
using CoolBro.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBro.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        services.AddScoped<ITimeOutCheckService, TimeOutCheckService>();
        services.AddScoped<IAdminService, AdminService>();
    }
}

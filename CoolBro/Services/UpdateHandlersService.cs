using CoolBro.Application.Interfaces;
using CoolBro.Application.Services.Session;
using CoolBro.Domain.Attributes;
using CoolBro.Extensions;
using CoolBro.Infrastructure.Data.Interfaces;
using CoolBro.KeyboardMarkups;
using CoolBro.Resources;
using CoolBro.UpdateHandlers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.RegularExpressions;
using Telegram.Bot;

namespace CoolBro.Services;

public class UpdateHandlersService(
    IServiceScopeFactory serviceScopeFactory,
    ITelegramBotClient client,
    ISessionRepository sessionRepository,
    ILogger<UpdateHandlersService> logger,
    IUserService userService,
    ISessionService sessionService)
{
    private static readonly IReadOnlyCollection<Type> UpdateHandlers = Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(type => typeof(UpdateHandlerBase).IsAssignableFrom(type) 
            && !type.IsAbstract)
        .ToArray();


    private readonly IServiceProvider _serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;

    public async Task HandleUpdateAsync(ExtendedUpdate update)
    {
        var user = await userService.GetOrCreateUserAsync(update.UserId, update.Username!);
        var session = await sessionService.GetOrCreateSessionAsync(user);
        var sessionManager = new SessionManager(sessionRepository, session);

        var hasMatchingHandler = false;
        var hasExecutedHandler = false;

        foreach (var handler in UpdateHandlers)
        {
            var handlerInstance = (UpdateHandlerBase)ActivatorUtilities.CreateInstance(_serviceProvider, handler);
            handlerInstance.Client = client;
            handlerInstance.Update = update;
            handlerInstance.User = user;
            handlerInstance.Session = sessionManager;

            var roleAttr = handler.GetCustomAttribute<RequiredRole>();

            if (roleAttr is not null && user.Role < roleAttr.Role)
                continue;

            hasMatchingHandler = true;

            if (await TryInvokeHandlerMethodAsync(handlerInstance))
            {
                hasExecutedHandler = true;
                break;
            }
        }

        if (!hasExecutedHandler)
        {
            await client.SendMessage(
                chatId: update.UserId,
                text: hasMatchingHandler
                    ? Messages.CommandNotFound
                    : Messages.NotEnoughPrivileges, 
                replyMarkup: ReplyMarkup.GoToMenu);
        }
    }

    private async Task<bool> TryInvokeHandlerMethodAsync(UpdateHandlerBase handler)
    {
        var methods = handler.GetType().GetMethods()
            .Where(m => m.GetCustomAttributes().Any())
            .ToArray();

        foreach (var method in methods)
        {
            var actionAttr = method.GetCustomAttribute<ActionAttribute>();
            if (actionAttr != null && handler.Session.CurrentState == actionAttr.Action)
            {
                if (handler.Update.Message != null)
                {
                    await (Task)method.Invoke(handler, null)!;
                    logger.LogInformation("Invoked: {Type}.{Method}",
                        method.DeclaringType!.Name,
                        method.Name);
                    return true;
                }
            }

            var callbackAttr = method.GetCustomAttribute<CallbackDataAttribute>();
            if (callbackAttr != null &&
                handler.Update.CallbackQuery?.Data == callbackAttr.Command)
            {
                await (Task)method.Invoke(handler, null)!;

                logger.LogInformation("Invoked: {Type}.{Method}",
                        method.DeclaringType!.Name,
                        method.Name);

                return true;
            }

            var callbackRegexAttr = method.GetCustomAttribute<CallbackDataRegexAttribute>();
            if (callbackRegexAttr != null &&
                handler.Update.CallbackQuery?.Data != null &&
                Regex.IsMatch(handler.Update.CallbackQuery.Data, callbackRegexAttr.Pattern, callbackRegexAttr.Options))
            {
                await (Task)method.Invoke(handler, null)!;
                
                logger.LogInformation("Invoked: {Type}.{Method}",
                    method.DeclaringType!.Name, 
                    method.Name);

                return true;
            }
        }

        return false;
    }
}

﻿using CoolBro.Application.Services;
using CoolBro.Domain.Attributes;
using CoolBro.Domain.Entities;
using CoolBro.Domain.Enums;
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
    IUserRepository userRepository,
    ISessionRepository sessionRepository,
    ILogger<UpdateHandlersService> logger,
    IValidator<User> userValidator,
    IValidator<State> stateValidator)
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
        var user = await GetOrCreateUserAsync(update);
        var session = await GetOrCreateSessionAsync(user);
        var sessionService = new SessionManager(sessionRepository, session);

        var hasMatchingHandler = false;
        var hasExecutedHandler = false;

        foreach (var handler in UpdateHandlers)
        {
            var handlerInstance = (UpdateHandlerBase)ActivatorUtilities.CreateInstance(_serviceProvider, handler);
            handlerInstance.Client = client;
            handlerInstance.Update = update;
            handlerInstance.User = user;
            handlerInstance.Session = sessionService;

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

    private async Task<User> GetOrCreateUserAsync(ExtendedUpdate update)
    {
        var user = await userRepository.GetByTelegramIdAsync(update.UserId);

        if (user is null)
        {
            user = new()
            {
                TelegramId = update.UserId,
                Username = update.Username ?? $"user_{update.UserId}",
                Role = Roles.User,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<Message>()
            };

            var validationResult = await userValidator.ValidateAsync(user);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            user = await userRepository.CreateAsync(user);
        }

        return user;
    }

    private async Task<State> GetOrCreateSessionAsync(User user)
    {
        var session = await sessionRepository.GetUserSessionByIdAsync(user.Id);

        if (session is null)
        {
            session = new()
            {
                UserId = user.Id,
                User = user,
                CurrentState = "Start"
            };
            
            var validationResult = await stateValidator.ValidateAsync(session);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await sessionRepository.SetUserSessionAsync(session);
        }

        return session;
    }
}

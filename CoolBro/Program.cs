using Telegram.Bot;
using Microsoft.Extensions.DependencyInjection;
using CoolBro.Infrastructure;
using CoolBro.Services;
using Telegram.Bot.Polling;
using CoolBro.UpdateHandlers;
using CoolBro.Application;



IServiceCollection services = new ServiceCollection();
services.AddInfrastructure();
services.AddApplication();
services.AddSingleton<ITelegramBotClient, TelegramBotClient>(t => new(token: "8179305311:AAG-F4TiSJD4ogbhcANYb2H0N7yTrnHJ67M"));
services.AddSingleton<UpdateHandlersService>();
services.AddSingleton<IUpdateHandler, TelegramUpdateHandler>();

using var buildProvider = services.BuildServiceProvider();

var client = buildProvider.GetRequiredService<ITelegramBotClient>();
var updateHandler = buildProvider.GetRequiredService<IUpdateHandler>();

client.StartReceiving(
    updateHandler: updateHandler,
    receiverOptions: new() { AllowedUpdates = [] });

Console.WriteLine("Бот запущен");

await Task.Delay(-1);
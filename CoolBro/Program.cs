using Telegram.Bot;
using Microsoft.Extensions.DependencyInjection;
using CoolBro.Infrastructure;
using CoolBro.Services;
using Telegram.Bot.Polling;
using CoolBro.UpdateHandlers;



IServiceCollection services = new ServiceCollection();
services.AddInfrastructure();
services.AddSingleton<ITelegramBotClient, TelegramBotClient>(t => new(token: "8179305311:AAG4wkHz0cj5fx0H1v-aiNokKBGxovbPJrY"));
services.AddSingleton<UpdateHandlersServices>();
services.AddSingleton<IUpdateHandler, TelegramUpdateHandler>();

using var buildProvider = services.BuildServiceProvider();

var client = buildProvider.GetRequiredService<ITelegramBotClient>();
var updateHandler = buildProvider.GetRequiredService<IUpdateHandler>();

client.StartReceiving(
    updateHandler: updateHandler,
    receiverOptions: new() { AllowedUpdates = [] });

Console.WriteLine("Бот запущен");

await Task.Delay(-1);
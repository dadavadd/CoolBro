using Telegram.Bot;
using Microsoft.Extensions.DependencyInjection;
using CoolBro.Infrastructure;
using CoolBro.Services;
using Telegram.Bot.Polling;
using CoolBro.UpdateHandlers;
using CoolBro.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.Extensions.Logging;



var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/coolbro-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


IServiceCollection services = new ServiceCollection();

services.AddInfrastructure(configuration, options =>
{
    options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddSerilog()))
           .EnableSensitiveDataLogging()
           .LogTo(message => Log.Debug(message), 
           [DbLoggerCategory.Database.Command.Name]);
});

services.AddApplication();

services.AddSingleton<ITelegramBotClient, TelegramBotClient>(t =>
    new(token: configuration["Telegram:BotToken"]!));
services.AddSingleton<UpdateHandlersService>();
services.AddSingleton<IUpdateHandler, TelegramUpdateHandler>();

services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(dispose: true);
});


try
{
    using var buildProvider = services.BuildServiceProvider();
    var client = buildProvider.GetRequiredService<ITelegramBotClient>();
    var updateHandler = buildProvider.GetRequiredService<IUpdateHandler>();

    Log.Information("Starting bot...");

    client.StartReceiving(
        updateHandler: updateHandler,
        receiverOptions: new() { AllowedUpdates = [] });

    Log.Information("Bot started successfully");

    await Task.Delay(-1);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
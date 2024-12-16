
using CoolBro.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using CoolBro.Application;

namespace CoolBro.UpdateHandlers;

public class TelegramUpdateHandler(
    UpdateHandlersServices updateHandlersServices
    ) : IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            var extendedUpdate = new ExtendedUpdate
            {
                Message = update.Message,
                EditedMessage = update.EditedMessage,
                ChannelPost = update.ChannelPost,
                EditedChannelPost = update.EditedChannelPost,
                InlineQuery = update.InlineQuery,
                CallbackQuery = update.CallbackQuery,
                MyChatMember = update.MyChatMember,
                ChatMember = update.ChatMember,
                ChatJoinRequest = update.ChatJoinRequest
            };

            await updateHandlersServices.HandleUpdateAsync(extendedUpdate);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(botClient, ex, HandleErrorSource.HandleUpdateError, cancellationToken);
        }
    }

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync($"Error handling update: {exception}");
    }
}

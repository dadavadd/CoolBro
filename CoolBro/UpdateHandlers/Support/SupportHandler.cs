using CoolBro.Domain.Attributes;
using CoolBro.Domain.Enums;
using CoolBro.Infrastructure.Data.Interfaces;
using CoolBro.KeyboardMarkups;
using CoolBro.Resources;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoolBro.UpdateHandlers.Support;

[RequiredRole(Roles.User)]
public class SupportHandler(
    IMessageRepository messageRepository) : UpdateHandlerBase
{
    [CallbackData("Support")]
    public async Task SupportTicketsHandleAsync()
    {
        await Client.EditMessageText(
            chatId: Update.UserId,
            messageId: Update.CallbackQuery!.Message!.MessageId,
            text: Messages.SupportMenu,
            replyMarkup: ReplyMarkup.GoBackToFromSupport);
    }

    [CallbackDataRegex(@"MyTickets_(\d+)")]
    public async Task MyTisketsHandlerAsync()
    {
        int page = int.Parse(
            Regex.Match(Update.CallbackQuery?.Data!, @"MyTickets_(\d+)").Groups[1].Value
            );

        const int pageSize = 5;
        var tickets = await messageRepository.GetMessagesByTelegramId(
            Update.UserId,
            take: pageSize,
            skip: page * pageSize
        );

        if (tickets is null || tickets.Count == 0)
        {
            await Client.EditMessageText(
                chatId: Update.UserId,
                messageId: Update.CallbackQuery!.Message!.MessageId,
                text: Messages.DontHaveTicketsYet,
                replyMarkup: ReplyMarkup.GoToMenu);
            return;
        }

        var buttons = tickets
            .Select((t, i) => 
                InlineKeyboardButton.WithCallbackData(
                    $"{page * pageSize + i + 1}", 
                    $"UserTicket_{t.Id}"))
            .ToList();

        if (page > 0)
            buttons.Add(InlineKeyboardButton.WithCallbackData(Buttons.Backward, $"MyTickets_{page - 1}"));

        if (tickets.Count > pageSize)
            buttons.Add(InlineKeyboardButton.WithCallbackData(Buttons.Forward, $"MyTickets_{page + 1}"));

        buttons.Add(InlineKeyboardButton.WithCallbackData(Buttons.GoBackToAccount, "Account"));

        await Client.EditMessageText(
            chatId: Update.UserId,
            messageId: Update.CallbackQuery!.Message!.MessageId,
            text: Messages.YourTickets,
            replyMarkup: new InlineKeyboardMarkup(buttons.Chunk(2)));
    }
}

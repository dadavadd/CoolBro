using CoolBro.Domain.Attributes;
using CoolBro.Domain.Enums;
using CoolBro.Infrastructure.Data.Interfaces;
using CoolBro.KeyboardMarkups;
using CoolBro.Resources;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoolBro.UpdateHandlers.Admin;

[RequiredRole(Roles.Admin)]
public class AdminSupportHandler(
    IMessageRepository messageRepository) : UpdateHandlerBase
{
    [CallbackDataRegex(@"AdminTickets_(\d+)")]
    public async Task HandleUserTicketsAsync()
    {
        int page = int.Parse(
            Regex.Match(Update.CallbackQuery?.Data!, @"AdminTickets_(\d+)").Groups[1].Value
        );

        const int pageSize = 5;
        var tickets = await messageRepository.GetAllNoReadMessages(
            take: pageSize,
            skip: page * pageSize
        );

        if (tickets is null || tickets.Count == 0)
        {
            await Client.SendMessage(
                chatId: Update.UserId,
                text: Messages.TicketsForAdminNotFound,
                replyMarkup: ReplyMarkup.GoToMenu);
            return;
        }

        var buttons = tickets
            .Select((t, i) =>
                InlineKeyboardButton.WithCallbackData(
                    $"{page * pageSize + i + 1}",
                    $"AdminTicket_{t.Id}"))
            .ToList();

        if (page > 0)
            buttons.Add(InlineKeyboardButton.WithCallbackData(Buttons.Backward, $"AdminTickets_{page - 1}"));

        if (tickets.Count > pageSize)
            buttons.Add(InlineKeyboardButton.WithCallbackData(Buttons.Forward, $"AdminTickets_{page + 1}"));

        buttons.Add(InlineKeyboardButton.WithCallbackData(Buttons.GoBackToAccount, "Account"));

        await Client.SendMessage(
            chatId: Update.UserId,
            text: Messages.TicketsSendedForAdmin,
            replyMarkup: new InlineKeyboardMarkup(buttons.Chunk(2)));
    }
}

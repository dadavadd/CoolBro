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
public class SupportHandler(IMessageRepository messageRepository) : UpdateHandlerBase
{
    [CallbackData("Support")]
    public async Task SupportTicketsHandleAsync()
    {
        await Client.SendMessage(
            chatId: Update.UserId,
            text: Messages.SupportMenu,
            replyMarkup: ReplyMarkup.GoBackToFromSupport);
    }

    [CallbackDataRegex(@"MyTickets_(\d+)")]
    public async Task MyTisketsHandlerAsync()
    {
        int page = int.Parse(Regex.Match(Update.CallbackQuery?.Data!, @"MyTickets_(\d+)").Groups[1].Value);

        const int pageSize = 5;
        var tickets = await messageRepository.GetMessagesByTelegramId(
            Update.UserId,
            take: pageSize,
            skip: page * pageSize
        );

        if (tickets is null || tickets.Count == 0)
        {
            await Client.SendMessage(
                chatId: Update.UserId,
                text: Messages.DontHaveTicketsYet,
                replyMarkup: ReplyMarkup.GoToMenu
            );
            return;
        }

        var buttons = tickets
             .Select((ticket, index) =>
                 InlineKeyboardButton.WithCallbackData(
                     $"{(page * pageSize) + index + 1}",
                     $"Ticket_{ticket.Id}"
                 )
             ).ToList();

        if (page > 0)
            buttons.Add(InlineKeyboardButton.WithCallbackData(Buttons.Backward, $"MyTickets_{page - 1}"));

        if (tickets.Count > pageSize)
            buttons.Add(InlineKeyboardButton.WithCallbackData(Buttons.Forward, $"MyTickets_{page + 1}"));

        var allButtons = buttons.Concat([InlineKeyboardButton.WithCallbackData(Buttons.GoBackToAccount, "Account")]).ToList();
        var keyboard = new InlineKeyboardMarkup(allButtons.Chunk(2));

        await Client.SendMessage(
            chatId: Update.UserId,
            text: Messages.YourTickets,
            replyMarkup: keyboard
        );
    }

    [CallbackDataRegex(@"Ticket_(\d+)")]
    public async Task HandleSpecificTicketAsync()
    {
        var ticketId = int.Parse(
            Regex.Match(Update.CallbackQuery!.Data!, @"Ticket_(\d+)").Groups[1].Value
        );

        var ticket = await messageRepository.GetMessagesById(ticketId, take: 1, skip: 0);

        if (ticket == null || ticket.Count == 0)
        {
            await Client.SendMessage(
                chatId: Update.UserId,
                text: Messages.TicketNotFound,
                replyMarkup: ReplyMarkup.GoToMenu
            );
            return;
        }

        Session.SetData(new Dictionary<string, object>
        {
            ["TicketId"] = ticketId,
            ["CreatedAt"] = ticket[0].CreatedAt
        });

        await Client.SendMessage(
            chatId: Update.UserId,
            text: string.Format(
                Messages.TicketInfo,
                ticketId,
                ticket[0].Content,
                ticket[0].IsRead ? $"✅{Buttons.Yes}" : $"❌{Buttons.No}",
                $"{ticket[0].CreatedAt:yyyy-MM-dd HH:mm}"),
            replyMarkup: ReplyMarkup.GoBackOrDeleteTicket
        );
    }
}

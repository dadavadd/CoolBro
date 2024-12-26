using CoolBro.Domain.Attributes;
using CoolBro.Domain.Enums;
using CoolBro.Infrastructure.Data.Interfaces;
using CoolBro.KeyboardMarkups;
using CoolBro.Resources;
using System.Text.RegularExpressions;
using Telegram.Bot;

namespace CoolBro.UpdateHandlers.Admin.Support;

[RequiredRole(Roles.Admin)]
public class ManageAdminTicketHandler(
    IMessageRepository messageRepository) : UpdateHandlerBase
{
    [CallbackDataRegex(@"AdminTicket_(\d+)")]
    public async Task HandleAdminTicketAsync()
    {
        var ticketId = int.Parse(
            Regex.Match(Update.CallbackQuery!.Data!, @"AdminTicket_(\d+)").Groups[1].Value
            );

        var ticket = await messageRepository.GetMessagesById(ticketId, take: 1, skip: 0);

        if (ticket == null || ticket.Count == 0)
        {
            await Client.EditMessageText(
                chatId: Update.UserId,
                messageId: Update.CallbackQuery!.Message!.MessageId,
                text: Messages.TicketNotFound,
                replyMarkup: ReplyMarkup.GoToMenu);
            return;
        }

        Session.SetData(new Dictionary<string, object>
        {
            ["TicketId"] = ticketId,
            ["CreatedAt"] = ticket[0].CreatedAt,
            ["BotMessageId"] = Update.CallbackQuery!.Message!.MessageId
        });

        await Client.EditMessageText(
            chatId: Update.UserId,
            messageId: Update.CallbackQuery!.Message!.MessageId,
            text: string.Format(
                Messages.AdminTicket,
                ticket[0].Id,
                ticket[0].Content,
                ticket[0].CreatedAt,
                ticket[0].Response),
            replyMarkup: ReplyMarkup.TicketRead);
    }
}

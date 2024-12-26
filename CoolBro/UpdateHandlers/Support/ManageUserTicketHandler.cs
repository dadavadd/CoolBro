using CoolBro.Domain.Attributes;
using CoolBro.Domain.Enums;
using CoolBro.Infrastructure.Data.Interfaces;
using CoolBro.KeyboardMarkups;
using CoolBro.Resources;
using System.Text.RegularExpressions;
using Telegram.Bot;

namespace CoolBro.UpdateHandlers.Support;

[RequiredRole(Roles.User)]
public class ManageUserTicketHandler(
    IMessageRepository messageRepository) : UpdateHandlerBase
{
    [CallbackDataRegex(@"UserTicket_(\d+)")]
    public async Task HandleSpecificTicketAsync()
    {
        var ticketId = int.Parse(
            Regex.Match(Update.CallbackQuery!.Data!, @"UserTicket_(\d+)").Groups[1].Value
        );

        var ticket = await messageRepository.GetMessagesById(ticketId, take: 1, skip: 0);

        if (ticket == null || ticket.Count == 0)
        {
            await Client.EditMessageText(
                chatId: Update.UserId,
                messageId: Update.CallbackQuery!.Message!.MessageId,
                text: Messages.TicketNotFound,
                replyMarkup: ReplyMarkup.AdminButtons);
            return;
        }

        Session.SetData(new Dictionary<string, object>
        {
            ["TicketId"] = ticketId,
            ["CreatedAt"] = ticket[0].CreatedAt
        });

        var replyText = ticket[0].IsRead ? 

            string.Format(
                Messages.AnswerTicketInfo,
                ticketId,
                ticket[0].Content,
                $"✅{Buttons.Yes}",
                $"{ticket[0].CreatedAt:yyyy-MM-dd HH:mm}",
                ticket[0].Response)
            :            
            string.Format(
                Messages.TicketInfo,
                ticketId,
                ticket[0].Content,
                $"❌{Buttons.No}",
                $"{ticket[0].CreatedAt:yyyy-MM-dd HH:mm}");


        await Client.EditMessageText(
            chatId: Update.UserId,
            messageId: Update.CallbackQuery!.Message!.MessageId,
            text: replyText,
            replyMarkup: ReplyMarkup.GoBackOrDeleteTicket
        );
    }
}

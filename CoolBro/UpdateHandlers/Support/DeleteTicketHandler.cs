using CoolBro.Infrastructure.Data.Interfaces;
using CoolBro.Domain.Attributes;
using CoolBro.KeyboardMarkups;
using CoolBro.Resources;
using CoolBro.Domain.Enums;
using Telegram.Bot;
using CoolBro.Application.Interfaces;

namespace CoolBro.UpdateHandlers.Support;

[RequiredRole(Roles.User)]
public class DeleteTicketHandler(
    IMessageRepository messageRepository,
    ITimeOutCheckService timeOutCheckService) : UpdateHandlerBase
{
    [CallbackData("DeleteTicket")]
    public async Task DeleteTicketHandleAsync()
    {
        await Client.SendMessage(
            chatId: Update.UserId,
            text: Messages.TicketDeleteConfirmed,
            replyMarkup: ReplyMarkup.DeleteTicketOrNo);
    }

    [CallbackData("TicketDeleteConfirmed")]
    public async Task DeleteTicketConfirmedAsync()
    {
        if (Session.Wrapper.GetOrDefault<int>("TicketId") == 0) return;
        if (DateTime.Parse(Session.Wrapper.GetOrDefault<string>("TicketCreatedAt")!) == DateTime.MinValue) return;

        var ticketId = Session.Wrapper.Get<int>("TicketId");
        var tickedDateTime = Session.Wrapper.Get<string>("TicketCreatedAt");

        if (!await timeOutCheckService.CheckMessageTimeOutAsync(ticketId, TimeSpan.FromHours(10)))
        {
            await Client.SendMessage(
                chatId: Update.UserId,
                text: string.Format(
                    Messages.TickedDeleteTimedOut,
                    tickedDateTime),
                replyMarkup: ReplyMarkup.GoToMenu);
            return;
        }

        await messageRepository.DeleteAsync(ticketId);

        await Session.ClearStateAsync();

        await Client.SendMessage(
            chatId: Update.UserId,
            text: Messages.TicketSuccesfullyDeleted,
            replyMarkup: ReplyMarkup.GoToMenu);
    }
}

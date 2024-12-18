using CoolBro.Infrastructure.Data.Interfaces;
using CoolBro.Domain.Attributes;
using CoolBro.KeyboardMarkups;
using CoolBro.Resources;
using CoolBro.Domain.Enums;
using Telegram.Bot;

namespace CoolBro.UpdateHandlers.Support;

[RequiredRole(Roles.User)]
public class DeleteTicketHandler(IMessageRepository messageRepository) : UpdateHandlerBase
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

        var ticketId = Session.Wrapper.Get<int>("TicketId");

        await messageRepository.DeleteAsync(ticketId);
        await Session.ClearStateAsync();

        await Client.SendMessage(
            chatId: Update.UserId,
            text: Messages.TicketSuccesfullyDeleted,
            replyMarkup: ReplyMarkup.GoToMenu);
    }
}

using CoolBro.Domain.Attributes;
using CoolBro.Domain.Enums;
using CoolBro.Infrastructure.Data.Interfaces;
using CoolBro.KeyboardMarkups;
using CoolBro.Resources;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoolBro.UpdateHandlers.Admin.Support;

[RequiredRole(Roles.Admin)]
public class ReplyToTicketHandler(
    IMessageRepository messageRepository) : UpdateHandlerBase
{
    [CallbackData("ReplyToTicket")]
    public async Task HandleReplyToTicketAsync()
    {
        if (Session.Wrapper.GetOrDefault<int>("TicketId") == default) return;
        if (Session.Wrapper.GetOrDefault<DateTime>("CreatedAt") == default) return;

        var ticketId = Session.Wrapper.Get<int>("TicketId");
        var ticket = await messageRepository.GetMessagesById(ticketId, 1, 0);
        
        if (ticket![0].IsRead)
        {
            await Client.EditMessageText(
                chatId: Update.Id,
                messageId: Update.CallbackQuery!.Message!.MessageId,
                text: Messages.TicketAlreadyBeenAnswered,
                replyMarkup: ReplyMarkup.GoToMenu);

            return;
        }

        await Session.SetStateAsync("SupportEntryReplyText");

        await Client.EditMessageText(
            chatId: Update.UserId,
            messageId: Update.CallbackQuery!.Message!.MessageId,
            text: Messages.EnterYourMessage,
            replyMarkup: ReplyMarkup.GoToMenu);
    }

    [Action("SupportEntryReplyText")]
    public async Task HandleSupportEntryReplyTextAsync()
    {
        if (Update.Message?.Text == null) return;
        if (Session.Wrapper.GetOrDefault<int>("TicketId") == default) return;
        if (Session.Wrapper.GetOrDefault<int>("BotMessageId") == default) return;

        await Client.DeleteMessage(
            chatId: Update.UserId,
            messageId: Update.Message.MessageId);

        var botMessageId = Session.Wrapper.Get<int>("BotMessageId");

        var ticketId = Session.Wrapper.Get<int>("TicketId");
        var ticket = await messageRepository.GetMessagesById(ticketId, 1, 0);

        ticket![0].IsRead = true;
        ticket![0].Response = Update.Message.Text;

        await messageRepository.UpdateMessageAsync(ticket![0]);

        await Client.EditMessageText(
            chatId: Update.UserId,
            messageId: botMessageId,
            text: Messages.TicketReplySuccesfully,
            replyMarkup: ReplyMarkup.AdminButtons);

        await Client.SendMessage(
            chatId: ticket![0].User.TelegramId,
            text: string.Format(
                Messages.AdminResponseForTicket,
                ticketId),
            replyMarkup: new InlineKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData(Buttons.GoToTicket, $"UserTicket_{ticketId}")));

        await Session.ClearStateAsync();
        await Session.SetStateAsync("Start");
    }
}

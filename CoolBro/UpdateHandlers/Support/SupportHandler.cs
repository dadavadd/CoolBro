using CoolBro.Domain.Attributes;
using CoolBro.Domain.Enums;
using CoolBro.Infrastructure.Data.Interfaces;
using CoolBro.KeyboardMarkups;
using CoolBro.Resources;
using Telegram.Bot;

namespace CoolBro.UpdateHandlers.Support;

[RequiredRole(Roles.User)]
public class SupportHandler(
    IMessageRepository messageRepository) : UpdateHandlerBase
{
    [CallbackData("Support")]
    public async Task SupportTicketsHandleAsync()
    {
        await Client.SendMessage(
            chatId: Update.UserId,
            text: Messages.EnterYouMessage,
            replyMarkup: AccountMarkup.GoBackToAccount);

        await Session.SetStateAsync("SupportTextEntry");
    }

    [Action("SupportTextEntry")]
    public async Task HandleSupportTextEntryAsync()
    {
        await Client.SendMessage(
            chatId: Update.UserId,
            text: $"Ваше сообщение: {Update.Message!.Text}",
            replyMarkup: AccountMarkup.GoBackToAccount);
    }
}

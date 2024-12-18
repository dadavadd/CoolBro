using CoolBro.Domain.Attributes;
using CoolBro.Domain.Enums;
using CoolBro.KeyboardMarkups;
using CoolBro.Resources;
using Telegram.Bot;

namespace CoolBro.UpdateHandlers.Account;

[RequiredRole(Roles.User)]
public class AccountHandler : UpdateHandlerBase
{
    [Action("Start")]
    [CallbackData("Account")]
    public async Task HandleAccountAsync()
    {
        await Session.SetStateAsync("Start");

        await Client.SendMessage(
            chatId: Update.UserId,
            text: string.Format(Messages.MainMenu, Update.FirstName),
            replyMarkup: ReplyMarkup.Account);
    }
}

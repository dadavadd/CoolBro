using CoolBro.Resources;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoolBro.KeyboardMarkups;

public partial class ReplyMarkup
{
    public static InlineKeyboardMarkup Account = new(new[]
    {
        InlineKeyboardButton.WithCallbackData(Buttons.Support, "Support")
    });

    public static InlineKeyboardMarkup GoToMenu = new(new[]
    {
        InlineKeyboardButton.WithCallbackData(Buttons.GoBackToAccount, "Account")
    });
}

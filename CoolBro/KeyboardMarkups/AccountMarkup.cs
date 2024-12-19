using CoolBro.Resources;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoolBro.KeyboardMarkups;

public partial class ReplyMarkup
{
    public static readonly InlineKeyboardMarkup Account = new(new[]
    {
        InlineKeyboardButton.WithCallbackData(Buttons.Support, "Support")
    });

    public static readonly InlineKeyboardMarkup GoToMenu = new(
    [
        [
            InlineKeyboardButton.WithCallbackData(Buttons.GoBackToAccount, "Account"),
        ]
    ]);
}

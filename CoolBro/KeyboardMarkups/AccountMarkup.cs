using CoolBro.Resources;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoolBro.KeyboardMarkups;

public partial class AccountMarkup
{
    public static InlineKeyboardMarkup Account = new(new[]
    {
        InlineKeyboardButton.WithCallbackData(Buttons.Support, "Support")
    });

    public static InlineKeyboardMarkup GoBackToAccount = new(new[]
    {
        InlineKeyboardButton.WithCallbackData(Buttons.GoBackToAccount, "Account")
    });
}

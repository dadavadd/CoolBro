using CoolBro.Resources;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoolBro.KeyboardMarkups;

public partial class ReplyMarkup
{
    public static readonly InlineKeyboardMarkup AdminButtons = new(
    [
        [
            InlineKeyboardButton.WithCallbackData(Buttons.TicketsForAdmin, "AdminTickets_0")
        ]
    ]);

    public static readonly InlineKeyboardMarkup TicketRead = new(
    [
        [
            InlineKeyboardButton.WithCallbackData(Buttons.Backward, "AdminTickets_0"),
            InlineKeyboardButton.WithCallbackData(Buttons.ReplyToTicket, "ReplyToTicket")
        ]
    ]);
}

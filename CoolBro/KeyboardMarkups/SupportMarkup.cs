using CoolBro.Resources;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoolBro.KeyboardMarkups;

public partial class ReplyMarkup
{
    public static InlineKeyboardMarkup GoBackToFromSupport = new(
    [
        [
            InlineKeyboardButton.WithCallbackData(Buttons.CreateTicket, "CreateSupportTicket"),
            InlineKeyboardButton.WithCallbackData(Buttons.MyTtickets, "MyTickets_0")
        ],
        [
            InlineKeyboardButton.WithCallbackData(Buttons.GoBackToAccount, "Account"),
        ]
    ]);

    public static InlineKeyboardMarkup GoBackOrDeleteTicket = new(
    [
        [
            InlineKeyboardButton.WithCallbackData(Buttons.DeleteTicket, "DeleteTicket")
        ],
        [
            InlineKeyboardButton.WithCallbackData(Buttons.GoBackToAccount, "Account"),
        ]
    ]);

    public static InlineKeyboardMarkup DeleteTicketOrNo = new(
    [
        [
            InlineKeyboardButton.WithCallbackData(Buttons.Yes, "TicketDeleteConfirmed"),
            InlineKeyboardButton.WithCallbackData(Buttons.No, "Account")
        ]
    ]);
}

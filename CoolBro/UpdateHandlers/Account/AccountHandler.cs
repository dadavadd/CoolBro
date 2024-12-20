﻿using CoolBro.Domain.Attributes;
using CoolBro.Domain.Enums;
using CoolBro.KeyboardMarkups;
using CoolBro.Resources;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoolBro.UpdateHandlers.Account;

public class AccountHandler : UpdateHandlerBase
{
    [Action("Start")]
    [CallbackData("Account")]
    public async Task HandleAccountAsync()
    {
        var baseButtons = ReplyMarkup.Account.InlineKeyboard.ToList();

        if (User.Role is Roles.Admin) 
            baseButtons.AddRange(ReplyMarkup.AdminButtons.InlineKeyboard);

        await Client.SendMessage(
            chatId: Update.UserId,
            text: string.Format(Messages.MainMenu, Update.FirstName),
            replyMarkup: new InlineKeyboardMarkup(baseButtons));
    }
}

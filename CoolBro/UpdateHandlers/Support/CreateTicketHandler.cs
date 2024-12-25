using CoolBro.Application.Interfaces;
using CoolBro.Domain.Attributes;
using CoolBro.Domain.Entities;
using CoolBro.Domain.Enums;
using CoolBro.Infrastructure.Data.Interfaces;
using CoolBro.KeyboardMarkups;
using CoolBro.Resources;
using FluentValidation;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoolBro.UpdateHandlers.Support;

[RequiredRole(Roles.User)]
public class CreateTicketHandler(
    IMessageRepository messageRepository,
    ITimeOutCheckService timeOutCheckService,
    IAdminService adminService,
    IValidator<Message> messageValidator) : UpdateHandlerBase
{
    [CallbackData("CreateSupportTicket")]
    public async Task CreateTicketHandlerAsync()
    {
        var lastTicket = await messageRepository.GetMessagesByTelegramId(Update.UserId, 1, 0);

        if (lastTicket is null || lastTicket.Count == 0)
        {
            await AllowToCreateTicketAsync();
            return;
        }

        if (!await timeOutCheckService.CheckMessageTimeOutAsync(lastTicket![0].Id, TimeSpan.FromHours(10)))
        {
            await Client.SendMessage(
                chatId: Update.UserId,
                text: string.Format(
                    Messages.TicketTimedOut,
                    $"{lastTicket![0].CreatedAt:yyyy-MM-dd HH:mm}"),
                replyMarkup: ReplyMarkup.GoToMenu);
            return;
        }

        await AllowToCreateTicketAsync();
    }

    private async Task AllowToCreateTicketAsync()
    {
        await Session.SetStateAsync("SupportTextEntry");

        Session.SetData(new Dictionary<string, object>
        {
            ["CreatedAt"] = DateTime.UtcNow,
        });

        await Client.SendMessage(
            chatId: Update.UserId,
            text: Messages.EnterYourMessage,
            replyMarkup: ReplyMarkup.GoToMenu);
    }

    [Action("SupportTextEntry")]
    public async Task HandleSupportTextEntryAsync()
    {
        if (Update.Message?.Text == null) return;
        if (Session.Wrapper.GetOrDefault<DateTime>("CreatedAt") == default) return;

        var createdAt = Session.Wrapper.Get<DateTime>("CreatedAt");
        var messageText = Update.Message!.Text!;

        var message = new Message
        {
            UserId = User.Id,
            Content = Update.Message!.Text!,
            CreatedAt = createdAt,
            IsRead = false
        };

        var validateResult = await messageValidator.ValidateAsync(message);

        if (!validateResult.IsValid)
        {
            await Client.SendMessage(
                chatId: Update.UserId,
                text: validateResult.ToString()!,
                replyMarkup: ReplyMarkup.GoToMenu);

            await Session.ClearStateAsync();
            await Session.SetStateAsync("Start");
            return;
        }

        var createdMessage = await messageRepository.CreateMessageAsync(message);

        await Client.SendMessage(
            chatId: Update.UserId,
            text: Messages.TicketIsCreated,
            replyMarkup: ReplyMarkup.GoToMenu);

        var admins = await adminService.GetAdmins();

        await Task.WhenAll(admins.Select(a =>
            Client.SendMessage(
                chatId: a.TelegramId,
                text: Messages.TicketCameForYou,
                replyMarkup: new InlineKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData(Buttons.GoToTicket, $"AdminTicket_{createdMessage.Id}")))));

        await Session.ClearStateAsync();
        await Session.SetStateAsync("Start");
    }
}

using CoolBro.Application.Interfaces;
using CoolBro.Domain.Attributes;
using CoolBro.Domain.Enums;
using CoolBro.Infrastructure.Data.Interfaces;
using CoolBro.KeyboardMarkups;
using CoolBro.Resources;
using Telegram.Bot;

namespace CoolBro.UpdateHandlers.Support;

[RequiredRole(Roles.User)]
public class CreateTicketHandler(
    IMessageRepository messageRepository,
    ITimeOutCheckService timeOutCheckService) : UpdateHandlerBase
{
    [CallbackData("CreateSupportTicket")]
    public async Task CreateTicketHandlerAsync()
    {
        var lastTicket = await messageRepository.GetMessagesByTelegramId(Update.UserId, 1, 0);

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

        await Session.SetStateAsync("SupportTextEntry");

        Session.SetData(new Dictionary<string, object>
        {
            ["CreatedAt"] = DateTime.UtcNow,
        });

        await Client.SendMessage(
            chatId: Update.UserId,
            text: Messages.EnterYouMessage,
            replyMarkup: ReplyMarkup.GoToMenu);
    }

    [Action("SupportTextEntry")]
    public async Task HandleSupportTextEntryAsync()
    {
        if (Update.Message?.Text is null) return;

        if (Session.Wrapper.GetOrDefault<DateTime>("CreatedAt") == default) return;

        var createdAt = Session.Wrapper.Get<DateTime>("CreatedAt");

        await messageRepository.CreateAsync(new()
        {
            UserId = User.Id,
            Content = Update.Message.Text,
            CreatedAt = createdAt,
            IsRead = false
        });

        await Session.ClearStateAsync();

        await Client.SendMessage(
            chatId: Update.UserId,
            text: Messages.TicketIsCreated,
            replyMarkup: ReplyMarkup.GoToMenu);
    }
}

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CoolBro.Application;

public class ExtendedUpdate : Update
{
    public ChatType? ChatType =>
        this switch
        {
            { Message: { } message } => message.Chat.Type,
            { EditedMessage: { } message } => message.Chat.Type,
            { ChannelPost: { } message } => message.Chat.Type,
            { EditedChannelPost: { } message } => message.Chat.Type,
            { InlineQuery: { } inlineQuery } => inlineQuery.ChatType,
            { CallbackQuery: { } callbackQuery } => callbackQuery.Message?.Chat.Type,
            { MyChatMember: { } chatMember } => chatMember.Chat.Type,
            { ChatMember: { } chatMember } => chatMember.Chat.Type,
            _ => null
        };

    public long UserId =>
        this switch
        {
            { Message: { } message } => message.From?.Id ?? 0,
            { EditedMessage: { } message } => message.From?.Id ?? 0,
            { InlineQuery: { } inlineQuery } => inlineQuery.From.Id,
            { CallbackQuery: { } callbackQuery } => callbackQuery.From.Id,
            { MyChatMember: { } chatMember } => chatMember.From.Id,
            { ChatMember: { } chatMember } => chatMember.From.Id,
            { ChatJoinRequest: { } chatJoinRequest } => chatJoinRequest.From.Id,
            _ => 0
        };

    public long ChatId =>
        this switch
        {
            { Message: { } message } => message.Chat.Id,
            { EditedMessage: { } message } => message.Chat.Id,
            { CallbackQuery: { } callbackQuery } => callbackQuery.Message?.Chat.Id ?? 0,
            { MyChatMember: { } chatMember } => chatMember.Chat.Id,
            { ChatMember: { } chatMember } => chatMember.Chat.Id,
            { ChatJoinRequest: { } chatJoinRequest } => chatJoinRequest.Chat.Id,
            _ => 0
        };

    public string? Username =>
        this switch
        {
            { Message: { } message } => message.From!.Username,
            { EditedMessage: { } message } => message.From!.Username,
            { CallbackQuery: { } callbackQuery } => callbackQuery.Message?.Chat.Username,
            _ => null
        };

    public string? FirstName =>
        this switch
        {
            { Message: { } message } => message.From?.FirstName,
            { EditedMessage: { } message } => message.From?.FirstName,
            { CallbackQuery: { } callbackQuery } => callbackQuery.Message?.Chat.FirstName,
            _ => null
        };

    public string? LastName =>
        this switch
        {
            { Message: { } message } => message.From?.LastName,
            { EditedMessage: { } message } => message.From?.LastName,
            { CallbackQuery: { } callbackQuery } => callbackQuery.Message?.Chat.LastName,
            _ => null
        };
}
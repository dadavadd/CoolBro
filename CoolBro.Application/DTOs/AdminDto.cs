using CoolBro.Domain.Enums;

namespace CoolBro.Application.DTOs;

public record AdminDto(int Id, long TelegramId, Roles Role);

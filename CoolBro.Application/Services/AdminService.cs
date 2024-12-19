using CoolBro.Domain.Enums;
using CoolBro.Application.DTOs;
using CoolBro.Application.Interfaces;
using CoolBro.Infrastructure.Data.Interfaces;
using AutoMapper;

namespace CoolBro.Application.Services;

public class AdminService(
    IUserRepository userRepository,
    IMapper mapper) : IAdminService
{
    public async Task<List<AdminDto>> GetAdmins() =>
        mapper.Map<List<AdminDto>>(await userRepository.GetAllAsync(u => u.Role == Roles.Admin));
}

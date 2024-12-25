using CoolBro.Domain.Enums;
using CoolBro.Application.DTOs;
using CoolBro.Application.Interfaces;
using CoolBro.Infrastructure.Data.Interfaces;
using AutoMapper;

namespace CoolBro.Application.Services.UserServices;

public class AdminService(
    IUserRepository userRepository,
    IMapper mapper) : IAdminService
{
    public async Task<IEnumerable<AdminDto>> GetAdmins() =>
        mapper.Map<IEnumerable<AdminDto>>(await userRepository.GetUsersByRoleAsync(Roles.Admin));
}

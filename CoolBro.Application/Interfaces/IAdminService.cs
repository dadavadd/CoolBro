using CoolBro.Application.DTOs;

namespace CoolBro.Application.Interfaces;

public interface IAdminService
{
    Task<List<AdminDto>> GetAdmins();
}

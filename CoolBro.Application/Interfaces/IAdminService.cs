using CoolBro.Application.DTOs;

namespace CoolBro.Application.Interfaces;

public interface IAdminService
{
    Task<IEnumerable<AdminDto>> GetAdmins();
}

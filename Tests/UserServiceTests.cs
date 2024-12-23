using CoolBro.Application.Interfaces;
using Moq;
using Moq.AutoMock;
using CoolBro.Application.DTOs;
using CoolBro.Domain.Enums;
using AutoMapper;
using CoolBro.Application.Services;
using CoolBro.Infrastructure.Data.Interfaces;
using CoolBro.Domain.Entities;

namespace Tests;

public class UserServiceTests
{
    private readonly AutoMocker _mocker;
    private readonly IAdminService _adminService;
    public UserServiceTests()
    {
        _mocker = new AutoMocker();
        _adminService = _mocker.CreateInstance<AdminService>();
    }

    [Fact]
    public async Task GetAdmins_ShouldReturnListOfAdmins()
    {
        List<AdminDto> mockAdmins = [new AdminDto(1, 6493853634, Roles.Admin)];

        var mockUsers = new List<User> { new() { Id = 1, TelegramId = 6493853634, Role = Roles.Admin } };

        _mocker.GetMock<IUserRepository>()
            .Setup(repo => repo.GetUsersByRoleAsync(Roles.Admin))
            .ReturnsAsync(mockUsers);

        _mocker.GetMock<IMapper>()
            .Setup(mapper => mapper.Map<IEnumerable<AdminDto>>(mockUsers))
            .Returns(mockAdmins);

        // Act
        var result = await _adminService.GetAdmins();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, admin => admin.Role == Roles.Admin);
    }
}
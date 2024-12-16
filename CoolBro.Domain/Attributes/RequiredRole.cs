

using CoolBro.Domain.Enums;

namespace CoolBro.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RequiredRole : Attribute
{
    public Roles Role { get; set; }

    public RequiredRole(Roles role)
    {
        Role = role;
    }
}

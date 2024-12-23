using CoolBro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolBro.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(t => t.Messages)
            .WithOne(u => u.User)
            .HasForeignKey(k => k.UserId)
            .HasPrincipalKey(k => k.Id)
            .IsRequired();
    }
}

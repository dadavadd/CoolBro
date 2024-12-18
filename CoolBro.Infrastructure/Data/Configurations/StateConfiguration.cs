using CoolBro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolBro.Infrastructure.Data.Configurations;

public class StateConfiguration : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.HasOne(t => t.User)
            .WithOne(t => t.Session)
            .HasForeignKey<State>(k => k.UserId)
            .HasPrincipalKey<User>(k => k.Id)
            .IsRequired();
    }
}

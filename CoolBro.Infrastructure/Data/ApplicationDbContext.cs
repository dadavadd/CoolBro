using CoolBro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoolBro.Infrastructure.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options
    ) : DbContext(options)
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(t => t.Messages)
            .WithOne(u => u.User)
            .HasForeignKey(k => k.UserId)
            .HasPrincipalKey(k => k.Id)
            .IsRequired();

        modelBuilder.Entity<State>()
            .HasOne(t => t.User)
            .WithOne(t => t.Session)
            .HasForeignKey<State>(k => k.UserId)
            .HasPrincipalKey<User>(k => k.Id)
            .IsRequired();

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<State> Session { get; set; }
}

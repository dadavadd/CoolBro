using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using CoolBro.Infrastructure.Data;

namespace CoolBro.Infrastructure.Data.Repositories;

public class RepositoryBase<T> where T : class
{
    protected readonly ApplicationDbContext Context;
    protected DbSet<T> Table { get; }
    public IQueryable<T> Query { get; }

    protected RepositoryBase(ApplicationDbContext context)
    {
        Context = context;
        Table = context.Set<T>();
        Query = Table.AsQueryable();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync() =>
        await Context.Database.BeginTransactionAsync();


    public async Task UseTransactionAsync(IDbContextTransaction dbTransaction) =>
        await Context.Database.UseTransactionAsync(dbTransaction.GetDbTransaction());

    public virtual async Task<T> InsertAsync(T entity)
    {
        var entry = await Table.AddAsync(entity);
        await SaveAsync();
        return entry.Entity;
    }

    public virtual async Task InsertRangeAsync(IEnumerable<T> entities)
    {
        await Table.AddRangeAsync(entities);
        await SaveAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        Table.Update(entity);
        await SaveAsync();
    }

    public virtual async Task RemoveAsync(T entity)
    {
        Table.Remove(entity);
        await SaveAsync();
    }

    public async Task RemoveAsync(IEnumerable<T> entities)
    {
        Table.RemoveRange(entities);
        await SaveAsync();
    }

    private async Task SaveAsync() =>
        await Context.SaveChangesAsync();
}

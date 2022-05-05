using Adventure.Domain;
using Adventure.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Adventure.Infrastructure.Repositories;
public abstract class GenericRepository<TEntity> where TEntity : BaseDomain
{
    private readonly AdventureDbContext _dbContext;

    protected GenericRepository(AdventureDbContext dbContext)
    {
        _dbContext = dbContext;
        DbSet = dbContext.Set<TEntity>();
    }

    protected DbSet<TEntity> DbSet { get; }

    public virtual async Task<TEntity> Add(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await SaveChanges();
        return entity;
    }

    public virtual Task<TEntity?> GetById(int id)
    {
        return DbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    private async Task SaveChanges()
    {
        await _dbContext.SaveChangesAsync();
    }
}
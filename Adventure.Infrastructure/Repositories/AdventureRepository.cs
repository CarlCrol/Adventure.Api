using Adventure.Core.Repositories;
using Adventure.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Adventure.Infrastructure.Repositories;

public class AdventureRepository : GenericRepository<Domain.Adventure>, IAdventureRepository
{
    public AdventureRepository(AdventureDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> Contains(string title)
    {
        return DbSet.AnyAsync(x => x.Title.ToLower() == title.ToLower());
    }

    public async Task<(IEnumerable<Domain.Adventure> Adventures, int MaxItems)> GetAdventures(int currentPage, int maxItems)
    {
        var maxCount = await DbSet.CountAsync();
        var adventures = await DbSet.OrderBy(x => x.Title).Skip(currentPage).Take(maxItems).ToListAsync();
        return (adventures, maxCount);
    }
}
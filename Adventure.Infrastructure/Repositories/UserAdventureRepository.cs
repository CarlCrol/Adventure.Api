using Adventure.Core.Repositories;
using Adventure.Domain;
using Adventure.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Adventure.Infrastructure.Repositories;

public class UserAdventureRepository : GenericRepository<Domain.UserAdventureAggregate>, IUserAdventureRepository
{
    public UserAdventureRepository(AdventureDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<UserAdventureAggregate?> GetById(int id)
    {
        return DbSet.Include(x => x.User).Include(x => x.Adventure).FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<(IEnumerable<UserAdventureAggregate> UserAdventureAggregates, int MaxItems)> GetUserAdventureAggregates(string username, int currentPage, int maxItems)
    {
        var query = DbSet.Where(x => x.User.Username.ToLower() == username);
        
        var itemCount = await query.CountAsync();
        var userAdventureAggregates = await query
                    .Include(x => x.User)
                    .Include(x => x.Adventure)
                    .OrderBy(x => x.AdventuredOn)
                    .Skip(currentPage).Take(maxItems)
                    .ToListAsync();

        return (userAdventureAggregates, itemCount);
    }
}
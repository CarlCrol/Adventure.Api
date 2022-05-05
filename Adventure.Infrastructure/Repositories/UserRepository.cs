using Adventure.Core.Repositories;
using Adventure.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Adventure.Infrastructure.Repositories;

public class UserRepository : GenericRepository<Domain.User>, IUserRepository
{
    public UserRepository(AdventureDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> Contains(string username)
    {
        return DbSet.AnyAsync(x => x.Username.ToLower() == username.ToLower());
    }
}
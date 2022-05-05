namespace Adventure.Core.Repositories;

public interface IUserAdventureRepository
{
    Task<Domain.UserAdventureAggregate> Add(Domain.UserAdventureAggregate userAdventureAggregate);
    Task<(IEnumerable<Domain.UserAdventureAggregate> UserAdventureAggregates, int MaxItems)> GetUserAdventureAggregates(string username, int currentPage, int maxItems);
}
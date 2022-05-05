namespace Adventure.Core.Repositories;

public interface IAdventureRepository
{
    Task<bool> Contains(string title);
    Task<Domain.Adventure> Add(Domain.Adventure adventure);
    Task<Domain.Adventure?> GetById(int id);
    Task<(IEnumerable<Domain.Adventure> Adventures, int MaxItems)> GetAdventures(int currentPage, int maxItems);
    
}
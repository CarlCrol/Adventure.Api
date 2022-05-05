namespace Adventure.Core.Repositories;

public interface IUserRepository
{
    Task<bool> Contains(string username);
    Task<Domain.User> Add(Domain.User user);
    Task<Domain.User?> GetById(int id);
}
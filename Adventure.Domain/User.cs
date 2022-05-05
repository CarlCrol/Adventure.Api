using Adventure.Domain.Exceptions;

namespace Adventure.Domain;

public class User : BaseDomain
{
    private string _username;
    public string Username => _username;
    private User(){}

    public User(string username)
    {
        if(string.IsNullOrWhiteSpace(username))
        {
            throw new ValidationException($"{nameof(username)} can't be null or empty.");
        }
        
        _username = username;
    }
}
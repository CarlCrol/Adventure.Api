using Adventure.Domain.Exceptions;
using Adventure.Domain.ValueTypes;

namespace Adventure.Domain;

public class UserAdventureAggregate : BaseDomain
{
    private readonly ICollection<SelectedRoute> _selectedRoutes;
    private readonly DateTime _adventuredOn;
    public int UserId { get; set; }
    public int AdventureId { get; set; }
    public DateTime AdventuredOn => _adventuredOn;
    public User User { get; set; }
    public Adventure Adventure { get; set; }
    public IEnumerable<SelectedRoute> SelectedRoutes => _selectedRoutes;

    private UserAdventureAggregate() { }
    
    public UserAdventureAggregate(DateTime adventuredOn, ICollection<SelectedRoute> selectedRoutes)
    {
        _selectedRoutes = selectedRoutes;
        _adventuredOn = adventuredOn;
    }
    public UserAdventureAggregate(User user, Adventure adventure, DateTime adventuredOn, ICollection<SelectedRoute> selectedRoutes)
    {
        if(user == null)
        {
            throw new ValidationException($"{nameof(user)} can't be null.");
        }

        if(adventure == null)
        {
            throw new ValidationException($"{nameof(adventure)} can't be null.");
        }
        
        if(!selectedRoutes?.Any() ?? true)
        {
            throw new ValidationException($"{nameof(selectedRoutes)} can't be null or empty.");
        }

        Route? configRoute = null;
        foreach(var selectedRoute in selectedRoutes)
        {
            if(configRoute == null)
            {
                configRoute = adventure.Routes.FirstOrDefault(x => x.Decision == selectedRoute.Decision && 
                                x.Comment == selectedRoute.Comment);
            }
            else
            {
                configRoute = configRoute.SubRoutes.FirstOrDefault(x => x.Decision == selectedRoute.Decision && 
                                x.Comment == selectedRoute.Comment);
            }

            if(configRoute == null)
            {
                throw new ValidationException($"{selectedRoute.Comment ?? selectedRoute.Decision} is not on the adventure routes.");
            }
        }
        _selectedRoutes = selectedRoutes;

        User = user;
        UserId = user.Id;
        Adventure = adventure;
        AdventureId = adventure.Id;
        _adventuredOn = adventuredOn;
    }
}
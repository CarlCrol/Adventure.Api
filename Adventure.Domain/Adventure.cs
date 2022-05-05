using Adventure.Domain.Exceptions;
using Adventure.Domain.ValueTypes;

namespace Adventure.Domain;

public class Adventure : BaseDomain
{
    private readonly string _title;

    private ICollection<Route> _routes = new HashSet<Route>();

    public IEnumerable<Route> Routes => _routes;
    public string Title => _title;
    
    private Adventure() { }

    public Adventure(string title, ICollection<Route> routes)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ValidationException($"{nameof(title)} should not be null or empty.");
        }

        if (!routes?.Any() ?? true)
        {
            throw new ValidationException($"{nameof(routes)} should not be null or empty.");
        }

        if (!routes.All(x => x.SubRoutes.Any()))
        {
            throw new ValidationException($"{nameof(routes)} top level route should have a sub-route.");
        }

        _title = title;
        _routes = routes;
    }
}

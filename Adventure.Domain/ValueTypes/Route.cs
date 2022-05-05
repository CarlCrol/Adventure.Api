using Adventure.Domain.Exceptions;

namespace Adventure.Domain.ValueTypes;

public class Route
{
    public string? Decision { get; private set; }
    public string? Comment { get; private set; }
    public IEnumerable<Route> SubRoutes { get; private set; }

    public Route(string? decision, string? comment, ICollection<Route> subRoutes)
    {
        if (string.IsNullOrWhiteSpace(decision) && string.IsNullOrWhiteSpace(comment))
        {
            throw new ValidationException($"{nameof(decision)} and {nameof(comment)} can't be null or empty.");
        }

        Decision = decision;
        Comment = comment;
        SubRoutes = subRoutes ?? new List<Route>();
    }
}
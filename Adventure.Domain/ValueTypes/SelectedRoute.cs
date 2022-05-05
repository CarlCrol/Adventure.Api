using Adventure.Domain.Exceptions;

namespace Adventure.Domain.ValueTypes;

public class SelectedRoute
{
    public string? Decision { get; private set; }
    public string? Comment { get; private set; }

    public SelectedRoute(string? decision, string? comment)
    {
        if (string.IsNullOrWhiteSpace(decision) && string.IsNullOrWhiteSpace(comment))
        {
            throw new ValidationException($"{nameof(decision)} and {nameof(comment)} can't be null or empty.");
        }

        Decision = decision;
        Comment = comment;
    }
}
namespace Adventure.Core;

public class ConflictException : Exception
{
    public ConflictException(string? message) : base(message)
    {
    }
}
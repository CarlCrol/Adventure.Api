namespace Adventure.Core;

public class ServiceResponse<T> : RequestResponse
{
    public T? Data { get; set; }
}
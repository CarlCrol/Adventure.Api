using Adventure.Core;
using Adventure.Core.Exceptions;
using Adventure.Domain.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace Adventure.Api;
public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case ValidationException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case ConflictException e:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;
                case NotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonConvert.SerializeObject(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}
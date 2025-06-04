using System.Net;
using System.Text.Json;

namespace MemosApplication.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
          StatusCode = context.Response.StatusCode,
          Message = message
        };

        string jsonResponse = JsonSerializer.Serialize(response);
        
        return context.Response.WriteAsync(jsonResponse);
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException e)
        {
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, e.Message);
        }
        catch (ArgumentNullException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (ApplicationException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, ex.Message);
        }

        catch (Exception e)
        {
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, e.Message);
        }
    }
}
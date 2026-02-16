using System.Text.Json;
using MySqlConnector;

namespace Seminario.Api.Middleware.ExceptionMiddleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (SeminarioException ex)
        {
            context.Response.Clear();
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = ex.Message
            };
            
            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
        catch (InvalidOperationException ex)
        {
            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = ex.Message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
        catch (MySqlException e)
        {
            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new { message = "Error con la base de datos o el EF, cominquese con sistemas: " + e.Message })
            );
        }
        catch (Exception e)
        {
            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new { message = "Error inesperado: " + e.Message })
            );
        }
    }
}
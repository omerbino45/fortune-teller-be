using FortuneTeller.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace FortuneTeller.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (EmailNotVerifiedException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.Forbidden, ex.Message, "EMAIL_NOT_VERIFIED");
        }
        catch (AppValidationException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (InvalidStateTransitionException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.Conflict, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode status, string message, string? code = null)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";
        var body = code is not null
            ? JsonSerializer.Serialize(new { error = message, code })
            : JsonSerializer.Serialize(new { error = message });
        await context.Response.WriteAsync(body);
    }
}

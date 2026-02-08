using System.Net;
using System.Text.Json;
using TruweatherCore.Models.DTOs;

namespace TruweatherAPI.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception for {Method} {Path}",
                context.Request.Method, context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, code, message) = exception switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, ApiErrorCodes.UnauthorizedAccess, "Unauthorized"),
            KeyNotFoundException => (HttpStatusCode.NotFound, ApiErrorCodes.NotFound, exception.Message),
            ArgumentException => (HttpStatusCode.BadRequest, ApiErrorCodes.InvalidInput, exception.Message),
            _ => (HttpStatusCode.InternalServerError, ApiErrorCodes.InternalServerError, "An unexpected error occurred")
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new ApiResponse<object>(
            Success: false,
            Message: message,
            Error: new ApiError(code, message, TraceId: context.TraceIdentifier)
        );

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}

using FoodFacilities.Models;

namespace FoodFacilitiesAPI.Middlewares;

/// <summary>
/// Middleware to handle errors inside the API
/// </summary>
public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invoke function that will execute the middleware
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }
    
    /// <summary>
    /// Handle the exception creating a response to client without exposing sensitive information
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = new ErrorResponse();
        if (exception is ArgumentException)
        {
            response.StatusCode = 400;
            response.Message = exception.Message;
        }
        else
        {
            response.StatusCode = 500;
            response.Message = "The server has encountered an unexpected error";
        }
        context.Response.StatusCode = response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}
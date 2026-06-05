namespace WaiterApp.Middleware;

public class GlobalErrorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalErrorMiddleware> _logger;

    public GlobalErrorMiddleware(RequestDelegate next, ILogger<GlobalErrorMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}

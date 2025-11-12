using System.Text.Json;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception whilst processing {Method} {Path}", context.Request.Method, context.Request.Path);
            var response = new { Message = "An unexpected error occurred.", TradeId = context.TraceIdentifier };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsJsonAsync(json);
        }
    }
}
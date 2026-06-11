using System.Diagnostics;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string correlationId = Guid.NewGuid().ToString("N")[..8];
        
        context.Response.Headers["X-Correlation-Id"] = correlationId;

        _logger.LogInformation(
            "Incoming Request: {Method} {Path} [CorrelationId: {CorrelationId}]", 
            context.Request.Method, 
            context.Request.Path, 
            correlationId
        );

        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        _logger.LogInformation(
            "Outgoing Response: {StatusCode} {Method} {Path} took {ElapsedMs}ms [CorrelationId: {CorrelationId}]", 
            context.Response.StatusCode, 
            context.Request.Method, 
            context.Request.Path, 
            stopwatch.ElapsedMilliseconds, 
            correlationId
        );
    }
}
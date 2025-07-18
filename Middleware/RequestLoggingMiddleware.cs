using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Middleware;


public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private  readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var method = context.Request.Method;
        var path = context.Request.Path;
        var query = context.Request.QueryString;
        _logger.LogInformation($"Method: {method}, Path: {path}, Query: {query}");

        foreach (var header in context.Request.Headers)
        {
            _logger.LogInformation($"Header: {header.Key}, Value: {header.Value}");
        }
        
        context.Request.EnableBuffering();
        using var reader = new StreamReader(context.Request.Body, leaveOpen: true) ;
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;
        if (!string.IsNullOrEmpty(body))
        {
            _logger.LogInformation("Body: {Body}");
        }
        
        var timestamp= DateTime.UtcNow;
        _logger.LogInformation($"Timestamp: {timestamp}");
        
        await _next(context);
        
        var statusCode = context.Response.StatusCode;
        _logger.LogInformation($"StatusCode: {statusCode}");
    }
    
}
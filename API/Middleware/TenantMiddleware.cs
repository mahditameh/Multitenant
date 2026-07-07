using System.Security.Claims;

namespace API.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantMiddleware> _logger;

    public TenantMiddleware(RequestDelegate next, ILogger<TenantMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? tenantId = null;
        
        // روش 1: از Header X-Tenant-ID
        if (context.Request.Headers.TryGetValue("X-Tenant-ID", out var headerTenantId))
        {
            tenantId = headerTenantId.ToString();
            _logger.LogDebug("Tenant found in header: {TenantId}", tenantId);
        }
        
        // روش 2: از Subdomain (مثلاً company1.localhost:5000)
        else if (context.Request.Host.Host.Contains("."))
        {
            var hostParts = context.Request.Host.Host.Split('.');
            if (hostParts.Length > 0)
            {
                var possibleTenant = hostParts[0];
                // بررسی کنیم که subdomain نباشد (مثل www)
                if (possibleTenant != "www" && possibleTenant != "localhost")
                {
                    tenantId = possibleTenant;
                    _logger.LogDebug("Tenant found in subdomain: {TenantId}", tenantId);
                }
            }
        }
        
        // روش 3: از JWT Token (اگر احراز هویت شده باشد)
        else if (context.User.Identity?.IsAuthenticated == true)
        {
            tenantId = context.User.Claims
                .FirstOrDefault(c => c.Type == "tenant_id" || c.Type == "TenantId")?
                .Value;
            
            if (!string.IsNullOrEmpty(tenantId))
                _logger.LogDebug("Tenant found in JWT: {TenantId}", tenantId);
        }
        
        // روش 4: از Query String (برای تست)
        else if (context.Request.Query.TryGetValue("tenantId", out var queryTenantId))
        {
            tenantId = queryTenantId.ToString();
            _logger.LogDebug("Tenant found in query string: {TenantId}", tenantId);
        }
        
        // روش 5: از Route (برای API key scenarios)
        else if (context.Request.RouteValues.TryGetValue("tenantId", out var routeTenantId))
        {
            tenantId = routeTenantId?.ToString();
            _logger.LogDebug("Tenant found in route: {TenantId}", tenantId);
        }
        
        // اگر هیچ روشی جواب نداد، مقدار پیش‌فرض
        if (string.IsNullOrWhiteSpace(tenantId) && context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Tenant is required. Send X-Tenant-ID header."
            });
            return;
        }

        tenantId ??= "default";
        
        // ذخیره TenantId در HttpContext.Items برای استفاده در TenantProvider
        context.Items["TenantId"] = tenantId;
        
        // اضافه کردن به Response Headers (برای دیباگ)
        context.Response.Headers.Append("X-Tenant-Id-Used", tenantId);
        
        _logger.LogInformation("Request Path: {Path}, Tenant: {TenantId}", 
            context.Request.Path, tenantId);
        
        // ادامه پردازش درخواست
        await _next(context);
    }
}

// Extension method برای ثبت آسان Middleware
public static class TenantMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantMiddleware>();
    }
}

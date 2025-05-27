using Microsoft.AspNetCore.Mvc.Filters;
using Loggers.service.Services;
using System.Security.Claims;

namespace CredWiseCustomer.Api
{
    public class ApiLoggingFilter : IActionFilter
    {
        private readonly AuditLogger _logger;

        public ApiLoggingFilter(AuditLogger logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var endpoint = httpContext.Request.Path;
            var method = httpContext.Request.Method;

            // Try to get user info from claims
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            var userType = httpContext.User.FindFirst("role")?.Value ?? "Anonymous";

            _logger.LogApiRequest(method, endpoint, $"API {method} request by {userType} (ID: {userId})");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Optionally log after the action executes
        }
    }
} 
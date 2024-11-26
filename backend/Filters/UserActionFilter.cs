using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace chatlaapp.Backend.Filters
{
    public class UserActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var username = context.HttpContext.Request.Headers["X-Username"].ToString();
            
            if (string.IsNullOrEmpty(username))
            {
                context.Result = new BadRequestObjectResult("Username is required");
                return;
            }

            // Add username to route data for easy access in controllers
            context.RouteData.Values["CurrentUsername"] = username;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action needed after execution
        }
    }
} 
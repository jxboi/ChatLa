using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace chatlaapp.Backend.Filters
{
    public class ImageValidationFilter : ActionFilterAttribute
    {
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly long maxFileSize = 5 * 1024 * 1024; // 5MB

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var file = context.HttpContext.Request.Form.Files.FirstOrDefault();
            
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(extension))
                {
                    context.Result = new BadRequestObjectResult("Invalid file type. Only .jpg, .jpeg, and .png are allowed.");
                    return;
                }

                if (file.Length > maxFileSize)
                {
                    context.Result = new BadRequestObjectResult("File size exceeds 5MB limit.");
                    return;
                }
            }
        }
    }
} 
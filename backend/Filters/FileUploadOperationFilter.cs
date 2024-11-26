using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;

namespace chatlaapp.Backend.Filters
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var formFileContent = context.MethodInfo.GetParameters()
                .Where(p => p.GetCustomAttributes(true).Any(attr => attr is FromFormAttribute));

            if (formFileContent.Any())
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = context.SchemaGenerator.GenerateSchema(
                                formFileContent.First().ParameterType, 
                                context.SchemaRepository
                            )
                        }
                    }
                };

                // Clear parameters since they're now part of the request body
                operation.Parameters?.Clear();
            }
        }
    }
} 
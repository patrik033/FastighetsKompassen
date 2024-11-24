using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FastighetsKompassen.API.Extensions
{
    public class MultipleFileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileUploadParameters = operation.Parameters
                .Where(p => p.Name == "files" && p.Schema.Type == "array")
                .ToList();

            foreach (var parameter in fileUploadParameters)
            {
                parameter.Schema.Type = "array";
                parameter.Schema.Items = new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                };
            }
        }
    }
}

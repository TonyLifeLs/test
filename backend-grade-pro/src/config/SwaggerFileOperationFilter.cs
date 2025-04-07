using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerFileOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParams = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile))
            .ToList();

        if (!fileParams.Any()) return;

        var formContent = new Dictionary<string, OpenApiSchema>();

        // Agregar el campo courseId (int)
        formContent["courseId"] = new OpenApiSchema
        {
            Type = "integer",
            Format = "int32"
        };

        // Agregar el campo file (IFormFile)
        formContent[fileParams[0].Name] = new OpenApiSchema
        {
            Type = "string",
            Format = "binary"
        };

        operation.Parameters.Clear();
        operation.RequestBody = new OpenApiRequestBody
        {
            Content =
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = formContent,
                        Required = new HashSet<string> { "courseId", fileParams[0].Name }
                    }
                }
            }
        };
    }
}
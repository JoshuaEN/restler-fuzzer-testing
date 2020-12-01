using Microsoft.OpenApi.Models;
using Server.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Text.Json;

namespace Server.OpenAPIFilters
{
    public class RestlerExampleFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var resterExamples = context.MethodInfo.GetCustomAttributes(true)
                .OfType<RestlerExampleAttribute>();

            if (resterExamples.Any())
            {
                var collection = new Microsoft.OpenApi.Any.OpenApiObject();
                foreach (var restlerExample in resterExamples)
                {
                    var example = context.MethodInfo.DeclaringType.GetMethod(restlerExample.ExampleGeneratorName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, null);
                    var jsonExample = JsonSerializer.Serialize(example);
                    var obj = new Microsoft.OpenApi.Readers.OpenApiStringReader().ReadFragment<Microsoft.OpenApi.Any.IOpenApiAny>(jsonExample, Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0, out _);
                    collection.Add(restlerExample.Title, new Microsoft.OpenApi.Any.OpenApiObject() { { "parameters", obj } });
                }

                operation.Extensions.Add("x-ms-examples", collection);
            }
        }
    }
}
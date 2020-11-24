using Microsoft.OpenApi.Models;
using Server.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace Server.OpenAPIFilters
{
    public class RestlerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var resterAnnotations = context.MethodInfo.GetCustomAttributes(true)
                .OfType<ResterAnnotationAttribute>();

            if (resterAnnotations.Any())
            {
                var collection = new Microsoft.OpenApi.Any.OpenApiArray();
                foreach (var resterAnnotation in resterAnnotations)
                {
                    var data = resterAnnotation.Data;
                    var obj = new Microsoft.OpenApi.Any.OpenApiObject();
                    if (data.ProducerEndpoint != null)
                    {
                        obj.Add("producer_endpoint", new Microsoft.OpenApi.Any.OpenApiString(data.ProducerEndpoint));
                    }

                    if (data.ProducerMethod != null)
                    {
                        obj.Add("producer_method", new Microsoft.OpenApi.Any.OpenApiString(data.ProducerMethod));
                    }

                    if (data.ProducerResourceName != null)
                    {
                        obj.Add("producer_resource_name", new Microsoft.OpenApi.Any.OpenApiString(data.ProducerResourceName));
                    }

                    if (data.ConsumerParam != null)
                    {
                        obj.Add("consumer_param", new Microsoft.OpenApi.Any.OpenApiString(data.ConsumerParam));
                    }
                    collection.Add(obj);
                }

                operation.Extensions.Add("x-restler-annotations", collection);
            }
        }
    }
}
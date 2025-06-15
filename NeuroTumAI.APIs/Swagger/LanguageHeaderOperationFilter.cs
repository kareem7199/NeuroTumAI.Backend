using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace NeuroTumAI.APIs.Swagger
{
	public class LanguageHeaderOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			operation.Parameters.Add(new OpenApiParameter
			{
				Name = "Accept-Language",
				In = ParameterLocation.Header,
				Required = true,
				Schema = new OpenApiSchema
				{
					Type = "string",
					Default = new OpenApiString("en"),
					Enum = new List<IOpenApiAny>
					{
						new OpenApiString("en"),
						new OpenApiString("ar")
					}
				}
			});
		}
	}
}

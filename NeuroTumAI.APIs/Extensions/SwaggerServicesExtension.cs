using Microsoft.OpenApi.Models;
using NeuroTumAI.APIs.Swagger;

namespace NeuroTumAI.APIs.Extensions
{
	public static class SwaggerServicesExtension
	{
		public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
		{

			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(options =>
			{
				// Add Accept-Language header globally
				options.OperationFilter<LanguageHeaderOperationFilter>();

				// Add JWT Bearer Authentication support in Swagger
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
				});

				// Make Swagger UI require a token for authorized endpoints
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						Array.Empty<string>()
					}
				});
			});

			return services;

		}

		public static WebApplication UseSwaggerMiddlewares(this WebApplication app)
		{

			app.UseSwagger();
			app.UseSwaggerUI();

			return app;
		}
	}
}

using System.Net;
using System.Text.Json;
using NeuroTumAI.APIs.Errors;
using NeuroTumAI.Core.Exceptions;

namespace NeuroTumAI.APIs.Middlewares
{
	public class ExceptionMiddleware : IMiddleware
	{
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IWebHostEnvironment _env;

		public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
		{
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext httpContext, RequestDelegate _next)
		{           //Take an action with the request
			try
			{
				await _next.Invoke(httpContext);
			}
			catch (Exception ex)
			{
				if (_env.IsDevelopment())
				{
					//DevelpopmentMode
					_logger.LogError(ex, ex.Message);
				}
				else
				{
					//Production Miode
					//log exception in database

				}
				await HandleExceptionAsync(httpContext, ex);
			}

			//Take an action with the response
		}
		private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
		{
			ApiResponse response;
			switch (ex)
			{
				case NotFoundException:
					httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
					httpContext.Response.ContentType = "application/json";
					response = new ApiResponse(404, ex.Message);
					await httpContext.Response.WriteAsync(response.ToString());
					break;

				case ValidationException validationException:
					httpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
					httpContext.Response.ContentType = "application/json";
					response = new ApiValidationErrorResponse(ex.Message) { Errors = validationException.Errors };
					await httpContext.Response.WriteAsync(response.ToString());
					break;

				case BadRequestException:
					httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
					httpContext.Response.ContentType = "application/json";
					response = new ApiResponse(400, ex.Message);
					await httpContext.Response.WriteAsync(response.ToString());
					break;

				case UnAuthorizedException:
					httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					httpContext.Response.ContentType = "application/json";
					response = new ApiResponse(401, ex.Message);
					await httpContext.Response.WriteAsync(response.ToString());
					break;

				default:
					response = _env.IsDevelopment() ?
							response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
							:
							response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

					httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					httpContext.Response.ContentType = "application/json";

					await httpContext.Response.WriteAsync(response.ToString());
					break;
			}
		}
	}
}

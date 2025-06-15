using System.Text.Json;

namespace NeuroTumAI.APIs.Errors
{
	public class ApiValidationErrorResponse : ApiResponse
	{
		public required IEnumerable<string> Errors { get; set; }
		public ApiValidationErrorResponse(string? Message = null) : base(422, Message)
		{

		}
		public class ValidationError
		{
			public required string Field { get; set; }

			public required IEnumerable<string> Errors { get; set; }
		}

		public override string ToString()
		{
			return JsonSerializer.Serialize(this, new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			});
		}


	}
}

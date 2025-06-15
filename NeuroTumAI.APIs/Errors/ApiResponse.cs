﻿using System.Text.Json;

namespace NeuroTumAI.APIs.Errors
{
	public class ApiResponse
	{
		public int StatusCode { get; set; }
		public string? Message { get; set; }

		public ApiResponse(int StatusCode, string? Message = null)
		{
			this.StatusCode = StatusCode;
			this.Message = Message ?? GetDefaultMessageForStatusCode(StatusCode);
		}

		private string? GetDefaultMessageForStatusCode(int StatusCode)
		{
			return StatusCode switch
			{
				400 => "A bad request, you have made",
				401 => "Authorized, you are not",
				404 => "Resource was not found",
				422 => "Validation error occurred.",
				500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate. hate leads to carrer change",
				_ => null
			};
		}

		public override string ToString()
		{
			return JsonSerializer.Serialize(this, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
		}
	}
}

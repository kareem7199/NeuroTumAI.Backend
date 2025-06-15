using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.CancerPrediction;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.Service.Services.CancerDetectionService
{
	public class CancerDetectionService : ICancerDetectionService
	{
		private readonly HttpClient _httpClient;

		public CancerDetectionService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<CancerPredictionResultDto> PredictCancerAsync(string imageUrl)
		{
			var payload = new { image_url = imageUrl };
			var response = await _httpClient.PostAsJsonAsync("http://ec2-51-21-208-180.eu-north-1.compute.amazonaws.com:5000/process", payload);
			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadFromJsonAsync<CancerPredictionResultDto>();
			return result!;
		}
	}
}

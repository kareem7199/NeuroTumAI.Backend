using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.CancerPrediction;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface ICancerDetectionService
	{
		Task<CancerPredictionResultDto> PredictCancerAsync(string imageUrl);
	}
}

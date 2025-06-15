using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NeuroTumAI.Core.Dtos.CancerPrediction
{
	public class PredictRequestDto
	{
		public IFormFile Image { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
	}
}

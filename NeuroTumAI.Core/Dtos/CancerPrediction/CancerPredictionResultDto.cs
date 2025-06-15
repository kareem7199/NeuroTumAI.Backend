using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.CancerPrediction
{
	public class CancerPredictionResultDto
	{
		public string Class { get; set; }
		public string Confidence { get; set; }
		public string Image { get; set; }
	}
}

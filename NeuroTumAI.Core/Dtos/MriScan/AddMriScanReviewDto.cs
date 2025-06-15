using System.ComponentModel.DataAnnotations;

namespace NeuroTumAI.Core.Dtos.MriScan
{
	public class AddMriScanReviewDto
	{
		[Required(ErrorMessage = "Findings_Required")]
		[MinLength(1, ErrorMessage = "Findings_MinLength")]
		public string Findings { get; set; }

		[Required(ErrorMessage = "Reasoning_Required")]
		[MinLength(1, ErrorMessage = "Reasoning_MinLength")]
		public string Reasoning { get; set; }

		[Required(ErrorMessage = "DetectionClass_Required")]
		[MinLength(1, ErrorMessage = "DetectionClass_MinLength")]
		public string DetectionClass { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Review
{
	public class AddReviewDto
	{
		[Required(ErrorMessage = "StarsRequired")]
		[Range(1, 5, ErrorMessage = "StarsRange")]
		public int Stars { get; set; }

		[Required(ErrorMessage = "CommentRequired")]
		[StringLength(1000, ErrorMessage = "CommentLength")]
		public string Comment { get; set; }

		[Required(ErrorMessage = "DoctorIdRequired")]
		public int DoctorId { get; set; }
	}
}

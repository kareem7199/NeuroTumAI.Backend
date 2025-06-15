using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Post
{
	public class AddCommentDto
	{

		[Required(ErrorMessage = "ContentRequired")]
		[StringLength(5000, MinimumLength = 1, ErrorMessage = "ContentLength")]
		public string Text { get; set; }
	}
}

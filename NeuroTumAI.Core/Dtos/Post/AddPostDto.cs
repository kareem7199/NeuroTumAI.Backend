using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Post
{
	public class AddPostDto
	{
		[Required(ErrorMessage = "TitleRequired")]
		[StringLength(100, MinimumLength = 3, ErrorMessage = "TitleLength")]
		public string Title { get; set; }

		[Required(ErrorMessage = "ContentRequired")]
		[StringLength(5000, MinimumLength = 10, ErrorMessage = "ContentLength")]
		public string Content { get; set; }
	}
}

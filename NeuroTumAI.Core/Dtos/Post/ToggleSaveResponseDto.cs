using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Post
{
	public class ToggleSaveResponseDto
	{
		public bool IsSaved { get; set; }
		public int PostId { get; set; }
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Post
{
	public class ToggleLikeResponseDto
	{
        public bool IsLiked { get; set; }
        public int PostId { get; set; }
    }
}

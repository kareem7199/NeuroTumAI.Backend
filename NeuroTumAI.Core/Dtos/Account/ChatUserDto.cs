using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Account
{
	public class ChatUserDto
	{
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string? ProfilePicture { get; set; }
    }
}

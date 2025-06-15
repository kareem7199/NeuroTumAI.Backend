using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Account;

namespace NeuroTumAI.Core.Dtos.Chat
{
	public class ConversationDto
	{
		public int Id { get; set; }
		public ChatUserDto User { get; set; }
	}
}

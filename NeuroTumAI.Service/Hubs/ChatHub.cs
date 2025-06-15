using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NeuroTumAI.Core.Entities.Chat_Aggregate;

namespace NeuroTumAI.Service.Hubs
{
	[Authorize]
	public class ChatHub : Hub
	{
	}
}

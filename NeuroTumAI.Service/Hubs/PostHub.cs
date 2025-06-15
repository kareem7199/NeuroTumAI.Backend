using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace NeuroTumAI.Service.Hubs
{
	[Authorize]
	public class PostHub: Hub
	{
	}
}

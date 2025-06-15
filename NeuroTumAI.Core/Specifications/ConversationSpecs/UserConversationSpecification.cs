using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Chat_Aggregate;

namespace NeuroTumAI.Core.Specifications.ConversationSpecs
{
	public class UserConversationSpecification : BaseSpecifications<Conversation>
	{
        public UserConversationSpecification(string userId)
            :base(C => C.FirstUserId == userId || C.SecondUserId == userId)
        {
            
        }
    }
}

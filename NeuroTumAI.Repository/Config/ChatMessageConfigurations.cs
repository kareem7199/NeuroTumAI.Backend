using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroTumAI.Core.Entities.Chat_Aggregate;

namespace NeuroTumAI.Repository.Config
{
	internal class ChatMessageConfigurations : IEntityTypeConfiguration<ChatMessage>
	{
		public void Configure(EntityTypeBuilder<ChatMessage> builder)
		{
			builder.HasOne(C => C.Sender)
				   .WithMany()
				   .HasForeignKey(C => C.SenderId);
		}
	}
}

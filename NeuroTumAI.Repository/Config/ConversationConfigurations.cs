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
	internal class ConversationConfigurations : IEntityTypeConfiguration<Conversation>
	{
		public void Configure(EntityTypeBuilder<Conversation> builder)
		{
			builder.HasOne(C => C.FirstUser)
				   .WithMany()
				   .HasForeignKey(C => C.FirstUserId)
				   .OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(C => C.SecondUser)
				   .WithMany()
	               .HasForeignKey(C => C.SecondUserId)
				   .OnDelete(DeleteBehavior.NoAction);
		}
	}
}

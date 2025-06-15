using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroTumAI.Core.Entities.Post_Aggregate;

namespace NeuroTumAI.Repository.Config
{
	internal class SavedPostConfigurations : IEntityTypeConfiguration<SavedPost>
	{
		public void Configure(EntityTypeBuilder<SavedPost> builder)
		{
			builder.HasOne(SP => SP.Post)
				.WithMany(P => P.Saves)
				.HasForeignKey(SP => SP.PostId)
				.OnDelete(DeleteBehavior.NoAction);
		}
	}
}

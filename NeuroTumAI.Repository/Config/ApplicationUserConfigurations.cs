using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Repository.Config
{
	internal class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(AU => AU.Gender)
				   .HasConversion(
				   (AUG) => AUG.ToString(),
				   (AUG) => (Gender)Enum.Parse(typeof(Gender), AUG)
				   );
		}
	}
}

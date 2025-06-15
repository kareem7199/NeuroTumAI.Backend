using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroTumAI.Core.Entities.MriScan;

namespace NeuroTumAI.Repository.Config
{
	public class DoctorMriAssignmentConfigurations : IEntityTypeConfiguration<DoctorMriAssignment>
	{
		public void Configure(EntityTypeBuilder<DoctorMriAssignment> builder)
		{
			builder.HasOne(a => a.Doctor)
				.WithMany(d => d.MriAssignments)
				.HasForeignKey(a => a.DoctorId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}

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
	public class MriScanConfigurations : IEntityTypeConfiguration<MriScan>
	{
		public void Configure(EntityTypeBuilder<MriScan> builder)
		{
			builder.HasOne(m => m.Patient)
				.WithMany()
				.HasForeignKey(m => m.PatientId)
				.OnDelete(DeleteBehavior.Restrict)
				.IsRequired();
		}
	}
}

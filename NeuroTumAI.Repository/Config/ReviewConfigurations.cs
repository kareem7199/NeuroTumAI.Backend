using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroTumAI.Core.Entities.Appointment;

namespace NeuroTumAI.Repository.Config
{
	internal class ReviewConfigurations : IEntityTypeConfiguration<Review>
	{
		public void Configure(EntityTypeBuilder<Review> builder)
		{
			builder.HasOne(r => r.Patient)
				.WithMany()
				.HasForeignKey(r => r.PatientId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(r => r.Doctor)
				.WithMany(D => D.Reviews)
				.HasForeignKey(r => r.DoctorId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}

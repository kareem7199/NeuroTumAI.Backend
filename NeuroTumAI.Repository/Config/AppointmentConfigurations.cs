using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroTumAI.Core.Entities.Appointment;

namespace NeuroTumAI.Repository.Config
{
	internal class AppointmentConfigurations : IEntityTypeConfiguration<Appointment>
	{
		public void Configure(EntityTypeBuilder<Appointment> builder)
		{
			builder.Property(A => A.Status)
				.HasConversion(
				(A) => A.ToString(),
				(A) => (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), A)
			);

			builder.HasOne(a => a.Clinic)
			.WithMany()
			.HasForeignKey(a => a.ClinicId)
			.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(a => a.Patient)
			.WithMany()
			.HasForeignKey(a => a.PatientId)
			.OnDelete(DeleteBehavior.Restrict);


			builder.HasOne(a => a.Doctor)
			.WithMany()
			.HasForeignKey(a => a.DoctorId)
			.OnDelete(DeleteBehavior.Cascade);
		}
	}
}

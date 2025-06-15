using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Entities.Contact_Us;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Repository.Config
{
    internal class ContactUSConfigurations : IEntityTypeConfiguration<ContactUsMessage>
    {
        public void Configure(EntityTypeBuilder<ContactUsMessage> builder)
        {
            builder.Property(C => C.Status)
                .HasConversion(
                (C) => C.ToString(),
                (C) => (MessageStatus)Enum.Parse(typeof(MessageStatus), C)
            );


            builder.HasOne(C => C.Patient)
            .WithMany()
            .HasForeignKey(C => C.PatientId)
            .OnDelete(DeleteBehavior.Restrict);





        }
    }
}

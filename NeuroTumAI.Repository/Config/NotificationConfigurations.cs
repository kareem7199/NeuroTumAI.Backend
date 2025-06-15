using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroTumAI.Core.Entities.Notification;

namespace NeuroTumAI.Repository.Config
{
	public class NotificationConfigurations : IEntityTypeConfiguration<Notification>
	{
		public void Configure(EntityTypeBuilder<Notification> builder)
		{
			builder.Property(N => N.Type)
				.HasConversion(
				(N) => N.ToString(),
				(N) => (NotificationType)Enum.Parse(typeof(NotificationType), N)
			);
		}
	}
}

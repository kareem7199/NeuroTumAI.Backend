using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;

namespace NeuroTumAI.Core.Specifications.SlotSpecs
{
	public class AvailableSlotSpecifications : BaseSpecifications<Slot>
	{
		public AvailableSlotSpecifications(DayOfWeek dayOfWeek, int clinicId, List<TimeOnly> times)
			: base(S => S.ClinicId == clinicId && S.DayOfWeek == dayOfWeek && S.IsAvailable == true && !times.Contains(S.StartTime))
		{
			AddOrderBy(S => S.StartTime);
		}
	}
}

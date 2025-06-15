using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;

namespace NeuroTumAI.Core.Specifications.SlotSpecs
{
	public class SlotSpecifications : BaseSpecifications<Slot>
	{
        public SlotSpecifications(DayOfWeek day , TimeOnly time , int clinicId)
            :base(S => S.ClinicId == clinicId && S.DayOfWeek == day && S.StartTime == time)
        {
        }

        public SlotSpecifications(DayOfWeek day, int clinicId)
            :base(S => S.DayOfWeek == day && S.ClinicId == clinicId)
        {
			AddOrderBy(S => S.StartTime);
		}
    }
}

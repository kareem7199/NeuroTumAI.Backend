using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Appointment;

namespace NeuroTumAI.Core.Specifications.AppointmentSpecs
{
	public class AppointmentWithPatientSpecifications: BaseSpecifications<Appointment>
	{
		public AppointmentWithPatientSpecifications(DateOnly date, int clinicId)
			: base(A => A.Date == date && A.Status != AppointmentStatus.Cancelled && A.ClinicId == clinicId)
		{
			Includes.Add(A => A.Patient);
			Includes.Add(A => A.Patient.ApplicationUser);
			AddOrderBy(A => A.StartTime);
		}
	}
}

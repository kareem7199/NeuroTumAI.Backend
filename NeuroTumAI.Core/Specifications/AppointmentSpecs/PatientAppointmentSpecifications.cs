using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Appointment;

namespace NeuroTumAI.Core.Specifications.AppointmentSpecs
{
	public class PatientAppointmentSpecifications : BaseSpecifications<Appointment>
	{
		public PatientAppointmentSpecifications(AppointmentSpecParams specParams, int patientId)
			: base(A => A.PatientId == patientId)
		{
			AddOrderByDesc(A => A.Date);
			AddOrderByDesc(A => A.StartTime);
			Includes.Add(A => A.Doctor);
			Includes.Add(A => A.Doctor.ApplicationUser);
			Includes.Add(A => A.Clinic);
			ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
		}
	}
}

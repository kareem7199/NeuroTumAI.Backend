using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Appointment;

namespace NeuroTumAI.Core.Specifications.AppointmentSpecs
{
	public class PatientAppointmentCountSpecifications : BaseSpecifications<Appointment>
	{
		public PatientAppointmentCountSpecifications(AppointmentSpecParams specParams, int patientId)
			: base(A => A.PatientId == patientId)
		{

		}
	}
}

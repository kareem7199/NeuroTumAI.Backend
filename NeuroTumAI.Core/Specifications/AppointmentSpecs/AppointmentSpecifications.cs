using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Appointment;

namespace NeuroTumAI.Core.Specifications.AppointmentSpecs
{
	public class AppointmentSpecifications : BaseSpecifications<Appointment>
	{

		public AppointmentSpecifications()
		{

		}

		public AppointmentSpecifications(int appointmentId)
			: base(A => A.Id == appointmentId)
		{
			Includes.Add(A => A.Patient);
			Includes.Add(A => A.Doctor);
		}

		public AppointmentSpecifications(List<DateOnly> dates, AppointmentStatus status)
			: base(A => dates.Contains(A.Date) && A.Status == status)
		{
		}

		public AppointmentSpecifications(List<DateOnly> dates, AppointmentStatus status, TimeOnly time)
			: base(A => dates.Contains(A.Date) && A.Status == status && A.StartTime == time)
		{
		}

		public AppointmentSpecifications(TimeOnly time, DateOnly date, int clinicId)
			: base(A => A.StartTime == time && A.Date == date && A.Status != AppointmentStatus.Cancelled && A.ClinicId == clinicId)
		{

		}
		public AppointmentSpecifications(int doctorId, int patientId)
			: base(A => A.DoctorId == doctorId && A.PatientId == patientId && A.Status == AppointmentStatus.Completed)
		{

		}
		public AppointmentSpecifications(DateOnly date, int clinicId)
			: base(A => A.Date == date && A.Status != AppointmentStatus.Cancelled && A.ClinicId == clinicId)
		{

		}
	}
}

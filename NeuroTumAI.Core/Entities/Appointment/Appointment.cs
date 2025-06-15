using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Entities.Appointment
{
	public class Appointment : BaseEntity
	{
		public DateOnly Date { get; set; }
		public TimeOnly StartTime { get; set; }
		public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }
        public int PatientId { get; set; }
		public Patient Patient { get; set; }
	}
}

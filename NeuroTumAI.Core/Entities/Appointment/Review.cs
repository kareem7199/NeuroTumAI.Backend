using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Entities.Appointment
{
	public class Review : BaseEntity
	{
		public int Stars { get; set; }
		public string Comment { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int DoctorId { get; set; }
		public Doctor Doctor { get; set; }
	}
}

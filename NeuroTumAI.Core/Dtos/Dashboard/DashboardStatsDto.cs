using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Dashboard
{
	public class DashboardStatsDto
	{
		public int Doctors { get; set; }
		public int PendingDoctors { get; set; }
		public int Clinics { get; set; }
		public int PendingClinics { get; set; }
		public int Appointments { get; set; }
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Appointments
{
	public class AppointmentToReturnDto
	{
        public int Id { get; set; }
		public DateOnly Date { get; set; }
		public TimeOnly StartTime { get; set; }
        public string Status { get; set; }
    }
}

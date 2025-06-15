using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Core.Entities
{
	public class Clinic : BaseEntity
	{
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string LicenseDocument { get; set; }
		public Point Location { get; set; }
		public bool IsApproved { get; set; } = false;
		public ICollection<Slot> Slots { get; set; } = new HashSet<Slot>();
 		public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}

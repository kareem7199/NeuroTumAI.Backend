using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.ContactUs
{
	public class ContactUsMessageToReturnDto
	{
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }
        public string PatientProfilePicture { get; set; }
    }
}

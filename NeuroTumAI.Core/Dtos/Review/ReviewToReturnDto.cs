using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Account;

namespace NeuroTumAI.Core.Dtos.Review
{
	public class ReviewToReturnDto
	{
        public int Id { get; set; }
        public int Stars { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt  { get; set; }
        public PublicPatientDto Patient { get; set; }
    }
}

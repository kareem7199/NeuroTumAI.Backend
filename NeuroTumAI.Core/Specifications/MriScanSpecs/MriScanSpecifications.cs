using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.MriScan;

namespace NeuroTumAI.Core.Specifications.MriScanSpecs
{
	public class MriScanSpecifications : BaseSpecifications<MriScan>
	{
		public MriScanSpecifications(DateTime date)
			: base(MS => !MS.IsReviewed && MS.UploadDate <= date)
		{

		}

		public MriScanSpecifications(int id)
			: base(MS => MS.Id == id)
		{
			Includes.Add(MS => MS.DoctorAssignments);
		}
	}
}

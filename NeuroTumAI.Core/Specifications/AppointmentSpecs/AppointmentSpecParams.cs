using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Specifications.AppointmentSpecs
{
	public class AppointmentSpecParams
	{
		private const int MaxPageSize = 15;
		private int pageSize;
		private string? search;

		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
		}

		public int PageIndex { get; set; } = 1;
	}
}

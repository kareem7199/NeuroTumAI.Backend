using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Specifications.NotificationSpecs
{
	public class NotificationSpecParams
	{
		private const int MaxPageSize = 15;
		private int pageSize;
		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
		}
		public int PageIndex { get; set; } = 1;
	}
}

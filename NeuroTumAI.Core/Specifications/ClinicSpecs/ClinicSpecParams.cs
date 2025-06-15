using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Specifications.ClinicSpecs
{
	public class ClinicSpecParams
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
		public string? Search { get => search; set => search = value?.ToLower(); }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
	}
}

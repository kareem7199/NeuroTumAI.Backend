using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos
{
	public class PaginationDto<T>
	{
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public int Count { get; set; }
		public int TotalPages => (int)Math.Ceiling((double)Count / PageSize);
		public IReadOnlyList<T> Data { get; set; }
		public PaginationDto(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
		{
			PageIndex = pageIndex;
			PageSize = pageSize;
			Data = data;
			Count = count;
		}
	}
}

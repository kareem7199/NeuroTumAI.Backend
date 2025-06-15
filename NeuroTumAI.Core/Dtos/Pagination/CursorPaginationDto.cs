using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Pagination
{
	public class CursorPaginationDto<T>
	{
		public IReadOnlyList<T> Data { get; set; }
		public int NextCursor { get; set; }

		public CursorPaginationDto(int nextCursor, IReadOnlyList<T> data)
		{
			Data = data;
			NextCursor = nextCursor;
		}
	}
}

namespace NeuroTumAI.Core.Dtos.Pagination
{
	public class PaginationParamsDto
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

using System.Linq.Expressions;
using NeuroTumAI.Core.Entities;

namespace NeuroTumAI.Core.Specifications
{
	public interface ISpecifications<T> where T : BaseEntity
	{
		public Expression<Func<T, bool>>? Criteria { get; set; }
		public List<Expression<Func<T, Object>>> Includes { get; set; }
		public Expression<Func<T, Object>> OrderBy { get; set; }
		public Expression<Func<T, Object>> OrderByDesc { get; set; }
		public int Skip { get; set; }
		public int Take { get; set; }
		public bool IsPaginationEnabled { get; set; }
	}
}

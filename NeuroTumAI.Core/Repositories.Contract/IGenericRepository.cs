using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Specifications;

namespace NeuroTumAI.Core.Repositories.Contract
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		Task<T?> GetAsync(int id);
		Task<IReadOnlyList<T>> GetAllAsync();
		Task<T?> GetWithSpecAsync(ISpecifications<T> spec);
		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
		Task<int> GetCountAsync(ISpecifications<T> spec);
		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);
		void RemoveRange(IEnumerable<T> entities);
	}
}

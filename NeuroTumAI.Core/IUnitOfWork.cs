using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Repositories.Contract;

namespace NeuroTumAI.Core
{
	public interface IUnitOfWork : IAsyncDisposable
	{
		IGenericRepository<T> Repository<T>() where T : BaseEntity;
		Task<int> CompleteAsync();
	}
}

using System.Collections;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Repositories.Contract;
using NeuroTumAI.Repository.Data;

namespace NeuroTumAI.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreContext _dbContext;
		private Hashtable _repositories;

		public UnitOfWork(StoreContext dbContext)
		{
			_dbContext = dbContext;
			_repositories = new Hashtable();
		}

		public async Task<int> CompleteAsync()
			=> await _dbContext.SaveChangesAsync();

		public async ValueTask DisposeAsync()
			=> await _dbContext.DisposeAsync();

		public IGenericRepository<T> Repository<T>() where T : BaseEntity
		{
			var key = typeof(T).Name;

			if (!_repositories.ContainsKey(key))
			{
				var repository = new GenericRepository<T>(_dbContext);

				_repositories.Add(key, repository);
			}

			return _repositories[key] as IGenericRepository<T>;
		}
	}
}

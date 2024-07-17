
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Task.DAL.IRepositories.Base
{
	
	public interface IBaseRepositoryAsync<TEntity, TPrimeryKey> : IAsyncDisposable
		where TEntity : class
	{
		
		
		Task<IQueryable<TEntity>> SetIncludedNavigationsListAsync(IQueryable<TEntity> source, IEnumerable<string> list);
		
		
		Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, string[] includedNavigationsList = null);


		Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, string[] includedNavigationsList = null);
	
		Task<TEntity> GetAsync(TPrimeryKey id);

		
		Task<IList<TEntity>> AddAsync(IEnumerable<TEntity> entityCollection);
		
		Task<TEntity> AddAsync(TEntity entity);

		
		Task<IQueryable<TEntity>> UpdateAsync(IEnumerable<TEntity> entityCollection);
		
		Task<TEntity> UpdateAsync(TEntity entity);


        System.Threading.Tasks.Task DeleteAsync(TPrimeryKey id);
       
        System.Threading.Tasks.Task DeleteAsync(IEnumerable<TPrimeryKey> idCollection);
        
        System.Threading.Tasks.Task DeleteAsync(TEntity entity);
       
	}
}

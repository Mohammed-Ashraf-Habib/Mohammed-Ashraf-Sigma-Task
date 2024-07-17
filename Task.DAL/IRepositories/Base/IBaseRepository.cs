using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Task.DAL.IRepositories.Base
{
	
	public interface IBaseRepository<TEntity, TPrimeryKey>
		where TEntity : class
	{
		
		IQueryable<TEntity> SetIncludedNavigationsList(IQueryable<TEntity> source, IEnumerable<string> list);
		
		
		IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, string[] includedNavigationsList = null);
		
		TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, string[] includedNavigationsList = null);
	
		TEntity Get(TPrimeryKey id);

	
		IList<TEntity> Add(IEnumerable<TEntity> entityCollection);
	
		TEntity Add(TEntity entity);

		IQueryable<TEntity> Update(IEnumerable<TEntity> entityCollection);
	
		TEntity Update(TEntity entity);

		void Delete(TPrimeryKey id);

		void Delete(IEnumerable<TPrimeryKey> idCollection);

		void Delete(TEntity entity);
	
		void Delete(IEnumerable<TEntity> entityCollection);
	}
}

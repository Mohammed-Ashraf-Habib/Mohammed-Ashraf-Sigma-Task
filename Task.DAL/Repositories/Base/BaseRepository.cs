
using Task.DAL.IRepositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace Task.DAL.Repositories.Base
{
	
	public class BaseRepository<TEntity, TPrimeryKey> :
		IDisposable,
		IBaseRepository<TEntity, TPrimeryKey>
		where TEntity : class
	{
		#region Data Members
		private DbContext _context;
		#endregion

		#region Constructors
		
		public BaseRepository(
			DbContext context
			)
		{
			this.Context = context;
		}
		#endregion

		public void Dispose()
		{
			this._context.Dispose();
		}

		#region Properties
		
		protected DbContext Context
		{
			get { return this._context; }
			set
			{
				this._context = value;
				this.Entities = this._context.Set<TEntity>();
			}
		}

		
		protected DbSet<TEntity> Entities { get; set; }
		#endregion

		#region IBaseRepository<TEntity, TPrimeryKey>
		
		public IQueryable<TEntity> SetIncludedNavigationsList(IQueryable<TEntity> source, IEnumerable<string> list)
		{
			if (source != null && list != null)
			{
				foreach (var item in list)
				{
					source = source.Include(item);
				}
			}

			return source;
		}
		
		
		public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, string[] includedNavigationsList = null)
		{

            var result = this.Entities.AsQueryable();

            if (predicate != null)
            {
                #region Set IncludedNavigationsList
                result =  SetIncludedNavigationsList(result, includedNavigationsList);
                #endregion

                #region Set Where Clause
                if (predicate != null)
                {
                    result = result.Where(predicate);
                }
                #endregion

            }

            return result;
        }
		
		public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, string[] includedNavigationsList = null)
		{
			var repo = this.Entities.AsQueryable();
			TEntity result = null;

			#region Set IncludedNavigationsList
			if (includedNavigationsList != null)
			{
				for (int i = 0; i < includedNavigationsList.Length; i++)
				{
					repo = repo.Include(includedNavigationsList[i]);
				}
			}
			#endregion

			#region Set Where Clause
			if (predicate != null)
			{
				result = repo.FirstOrDefault(predicate);
			}
			#endregion

			return result;

		}
	
		public virtual TEntity Get(TPrimeryKey id)
		{
			var result = this.Entities.Find(id);
			return result;
		}

		
		public virtual IList<TEntity> Add(IEnumerable<TEntity> entityCollection)
		{
			DateTime now = DateTime.Now;

			this.Entities.AddRange(entityCollection.ToList());
			return entityCollection.ToList();
		}
		
		public virtual TEntity Add(TEntity entity)
		{

			this.Entities.Add(entity);
			return entity;
		}

	
		public virtual IQueryable<TEntity> Update(IEnumerable<TEntity> entityCollection)
		{
			DateTime now = DateTime.Now;

			

			this.Entities.UpdateRange(entityCollection);
			return entityCollection.AsQueryable();
		}
		
		public virtual TEntity Update(TEntity entity)
		{
			
			

			this.Entities.Update(entity);
			return entity;
		}

		
		public virtual void Delete(TPrimeryKey id)
		{
			var entity = this.Entities.Find(id);
			this.Delete(entity);
		}

		public virtual void Delete(IEnumerable<TPrimeryKey> idCollection)
		{
			foreach (var id in idCollection)
			{
				this.Delete(id);
			}
		}
	
		public virtual void Delete(TEntity entity)
		{
			
				this.Entities.Remove(entity);
		}
	
		public virtual void Delete(IEnumerable<TEntity> entityCollection)
		{
			foreach (var entity in entityCollection)
			{
				this.Delete(entity);
			}
		}
		#endregion
	}
}

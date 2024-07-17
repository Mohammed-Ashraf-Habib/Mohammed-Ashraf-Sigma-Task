
using Task.DAL.IRepositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Task.DAL.Repositories.Base
{

	public class BaseRepositoryAsync<TEntity, TPrimeryKey> :
		IDisposable,
		IAsyncDisposable,
		IBaseRepositoryAsync<TEntity, TPrimeryKey>
		where TEntity : class
	{
		#region Data Members
		protected DbContext _context;
		
		#endregion

	
		public BaseRepositoryAsync(
			DbContext context
			)
		{
			this.Context = context;
		}

		public void Dispose()
		{
			this._context.Dispose();
		}

		public ValueTask DisposeAsync()
		{
			return this._context.DisposeAsync();
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

		
		public async Task<IQueryable<TEntity>> SetIncludedNavigationsListAsync(IQueryable<TEntity> source, IEnumerable<string> list)
		{
			return await System.Threading.Tasks.Task.Run(() =>
			{
				if (source != null && list != null)
				{
					foreach (var item in list)
					{
						source = source.Include(item);
					}
				}

				return source;
			});
		}
	
		public virtual async Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, string[] includedNavigationsList = null)
		{
			return await System.Threading.Tasks.Task.Run(async () =>
		   {
			   var result = this.Entities.AsQueryable();

			   if (predicate != null)
			   {
				   #region Set IncludedNavigationsList
				   result = await SetIncludedNavigationsListAsync(result, includedNavigationsList);
				   #endregion

				   #region Set Where Clause
				   if (predicate != null)
				   {
					   result = result.Where(predicate);
				   }
				   #endregion

			   }

			   return result;

		   });
		}
	
		public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, string[] includedNavigationsList = null)
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
				result = await repo.FirstOrDefaultAsync(predicate);
			}
			#endregion

			return result;
		}
	
		public virtual async Task<TEntity> GetAsync(TPrimeryKey id)
		{
			var result = await this.Entities.FindAsync(id);
			return result;
		}

		public virtual async Task<IList<TEntity>> AddAsync(IEnumerable<TEntity> entityCollection)
		{
			DateTime now = DateTime.Now;
			

			await this.Entities.AddRangeAsync(entityCollection);
			return entityCollection.ToList();
		}
		
		public virtual async Task<TEntity> AddAsync(TEntity entity)
		{
			DateTime now = DateTime.Now;
			

			var contextenttiy=await this.Entities.AddAsync(entity);
			return contextenttiy.Entity;
		}

		
		public virtual async Task<IQueryable<TEntity>> UpdateAsync(IEnumerable<TEntity> entityCollection)
		{
			return await System.Threading.Tasks.Task.Run(() =>
			{
				DateTime now = DateTime.Now;

				

				this.Entities.UpdateRange(entityCollection);
				return entityCollection.AsQueryable();
			});
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public virtual async Task<TEntity> UpdateAsync(TEntity entity)
		{
			return await System.Threading.Tasks.Task.Run(() =>
			{
			
				
				return this.Entities.Update(entity).Entity;
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public virtual async System.Threading.Tasks.Task DeleteAsync(TPrimeryKey id)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				var entity = this.Entities.Find(id);

				this.DeleteEntity(entity);
			});
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="idCollection"></param>
		/// <returns></returns>
		public virtual async System.Threading.Tasks.Task DeleteAsync(IEnumerable<TPrimeryKey> idCollection)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				foreach (var id in idCollection)
				{
					var entity = this.Entities.Find(id);

					this.DeleteEntity(entity);
				}
			});
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public virtual async System.Threading.Tasks.Task DeleteAsync(TEntity entity)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				this.DeleteEntity(entity);
			});
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="entityCollection"></param>
		/// <returns></returns>
		public virtual async System.Threading.Tasks.Task DeleteAsync(IEnumerable<TEntity> entityCollection)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				foreach (var entity in entityCollection)
				{
					this.DeleteEntity(entity);
				}
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		private void DeleteEntity(TEntity entity)
		{
			
				this.Entities.Remove(entity);
			
		}

    }
}

#region Using ...
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Task.DAL.Context;
using Task.DAL.IRepositories;
#endregion


namespace Task.DAL.Repositories
{
	
	public class UnitOfWorkAsync : IUnitOfWorkAsync
	{
		#region Data Members
		private TaskDbContext _context;
		#endregion

		#region Constructors
		
		public UnitOfWorkAsync(TaskDbContext context)
		{
			this._context = context;
		}
		#endregion

		#region IUnitOfWork	

		public async Task<int> CommitAsync()
		{
			var result = await this._context.SaveChangesAsync();
			return result;
		}
		#endregion
	}
}

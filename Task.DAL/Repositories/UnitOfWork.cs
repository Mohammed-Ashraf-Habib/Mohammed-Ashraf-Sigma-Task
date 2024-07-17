
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Task.DAL.Context;
using Task.DAL.IRepositories;



namespace Task.DAL.Repositories
{

	public class UnitOfWork : IUnitOfWork
	{
		#region Data Members
		private TaskDbContext _context;
		#endregion

		#region Constructors
		
		public UnitOfWork(TaskDbContext context)
		{
			this._context = context;
		}
		#endregion

		#region IUnitOfWork
	
		public int Commit()
		{
			var result = this._context.SaveChanges();
			return result;
		}		
		#endregion
	}
}

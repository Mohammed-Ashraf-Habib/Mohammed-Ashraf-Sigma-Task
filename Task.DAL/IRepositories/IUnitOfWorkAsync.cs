using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Task.DAL.IRepositories
{
	
	public interface IUnitOfWorkAsync
	{
		#region Methods
		
		Task<int> CommitAsync();
		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Task.DAL.IRepositories
{

	public interface IUnitOfWork
	{
		#region Methods
	
		int Commit();
		#endregion
	}
}

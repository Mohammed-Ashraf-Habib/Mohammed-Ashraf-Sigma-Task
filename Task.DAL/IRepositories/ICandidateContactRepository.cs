using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.DAL.Entity;
using Task.DAL.IRepositories.Base;

namespace Task.DAL.IRepositories
{
    public interface ICandidateContactRepository:IBaseRepositoryAsync<CandidateContact, long>
    {
    }
}

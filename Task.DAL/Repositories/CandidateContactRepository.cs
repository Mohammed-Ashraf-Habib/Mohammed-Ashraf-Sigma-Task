
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.DAL.Context;
using Task.DAL.Entity;
using Task.DAL.IRepositories;
using Task.DAL.Repositories.Base;

namespace Task.DAL.Repositories
{
    public class CandidateContactRepository : BaseRepositoryAsync<CandidateContact, long>, ICandidateContactRepository
    {
        public CandidateContactRepository(TaskDbContext context) : base(context)
        {
        }
    }
}

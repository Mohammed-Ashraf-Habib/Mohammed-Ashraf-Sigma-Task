using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.DAL.Context
{
    public class TaskDbContext: DbContext
    {
        public TaskDbContext(DbContextOptions options) : base(options)
        {
        }
       public DbSet<Entity.CandidateContact> CandidateContacts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Mapping.CandidateContactMap());
        }

    }
}

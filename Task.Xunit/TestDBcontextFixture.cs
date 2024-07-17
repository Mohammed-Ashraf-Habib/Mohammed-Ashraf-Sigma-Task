using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Task.DAL.Context;
using Task.DAL.Entity;

namespace Task.Xunit
{
    public class TestDBcontextFixture
    {
        private const string ConnectionString = @"Server=.;Database=TaskDBTest;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true;";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public TestDBcontextFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        context.AddRange(
                            new CandidateContact { FirstName= "Candidate1",LastName= "Candidate1",
                            Email= "Candidate1",CallTime= "Candidate1",Comment= "Candidate1",GitHub= "Candidate1",LinkedIn= "Candidate1" ,PhoneNumber= "Candidate1"
                            
                            },
                            new CandidateContact { 
                            FirstName= "Candidate2",LastName= "Candidate2",
                            Email= "Candidate2",CallTime= "Candidate2",Comment= "Candidate2",GitHub= "Candidate2",LinkedIn= "Candidate2" ,PhoneNumber= "Candidate2"
                            
                            });
                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public TaskDbContext CreateContext()
            => new TaskDbContext(
                new DbContextOptionsBuilder<TaskDbContext>()
                    .UseSqlServer(ConnectionString)
                    .Options);
    }
}

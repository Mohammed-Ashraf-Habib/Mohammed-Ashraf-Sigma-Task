using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using System.Globalization;
using Task.Business;
using Task.Business.DTO;
using Task.Business.Service;
using Task.DAL.Context;
using Task.DAL.Repositories;
using Xunit;

namespace Task.Xunit
{
    public class CandidateContactServiceTest: IClassFixture<TestDBcontextFixture>
    {
        private readonly TestDBcontextFixture _TestDBcontextFixture;
        private TaskDbContext _context;
        public CandidateContactServiceTest(TestDBcontextFixture testDBcontextFixture)
        {
            _TestDBcontextFixture = testDBcontextFixture;
        }
        private CandidateContactService PrepareService()
        {
            var context = _TestDBcontextFixture.CreateContext();
            Assert.NotNull(context);
            _context = context;
            CandidateContactRepository candidateContactRepository = new CandidateContactRepository(context);
            Assert.NotNull(candidateContactRepository);
            UnitOfWorkAsync unitOfWorkAsync = new UnitOfWorkAsync(context);
            Assert.NotNull(unitOfWorkAsync);
            AutoMapper.MapperConfiguration mapperConfiguration = new AutoMapper.MapperConfiguration(cfg => { cfg.AddProfile<Profile>(); });
            Assert.NotNull(mapperConfiguration);
            AutoMapper.Mapper mapper = new AutoMapper.Mapper(mapperConfiguration);
            Assert.NotNull(mapper);
            MemoryCacheOptions memoryCacheOptions = new MemoryCacheOptions();
            Assert.NotNull(memoryCacheOptions);
            memoryCacheOptions.Clock = new SystemClock();
            memoryCacheOptions.ExpirationScanFrequency = TimeSpan.FromSeconds(1);

            MemoryCache memoryCache = new MemoryCache(memoryCacheOptions);
            CandidateContactService service = new CandidateContactService(candidateContactRepository, unitOfWorkAsync, mapper, memoryCache);
            Assert.NotNull(service);
            return service;
        }
        private void compare(CandidateContactDTO candidateContactDTO, Task.DAL.Entity.CandidateContact candidateContact)
        {
            Assert.NotNull(candidateContactDTO);
            Assert.NotNull(candidateContact);
            Assert.Equal(candidateContactDTO.Id, candidateContact.Id);
            Assert.Equal(candidateContactDTO.FirstName, candidateContact.FirstName);
            Assert.Equal(candidateContactDTO.LastName, candidateContact.LastName);
            Assert.Equal(candidateContactDTO.Email, candidateContact.Email);
            Assert.Equal(candidateContactDTO.CallTime, candidateContact.CallTime);
            Assert.Equal(candidateContactDTO.Comment, candidateContact.Comment);
            Assert.Equal(candidateContactDTO.GitHub, candidateContact.GitHub);
            Assert.Equal(candidateContactDTO.LinkedIn, candidateContact.LinkedIn);
            Assert.Equal(candidateContactDTO.PhoneNumber, candidateContact.PhoneNumber);
        }
        private void compareDTOS(CandidateContactDTO candidateContactDTO, CandidateContactDTO candidateContact)
        {
            Assert.NotNull(candidateContactDTO);
            Assert.NotNull(candidateContact);
            Assert.Equal(candidateContactDTO.Id, candidateContact.Id);
            Assert.Equal(candidateContactDTO.FirstName, candidateContact.FirstName);
            Assert.Equal(candidateContactDTO.LastName, candidateContact.LastName);
            Assert.Equal(candidateContactDTO.Email, candidateContact.Email);
            Assert.Equal(candidateContactDTO.CallTime, candidateContact.CallTime);
            Assert.Equal(candidateContactDTO.Comment, candidateContact.Comment);
            Assert.Equal(candidateContactDTO.GitHub, candidateContact.GitHub);
            Assert.Equal(candidateContactDTO.LinkedIn, candidateContact.LinkedIn);
            Assert.Equal(candidateContactDTO.PhoneNumber, candidateContact.PhoneNumber);
        }
        [Fact]
        public void CandidateContactService__Create__success()
        {
            CandidateContactService service =PrepareService();
            Assert.NotNull(service);
            var newCandidateContact = new Task.Business.DTO.CandidateContactDTO
            {
                FirstName = "Candidate3",
                LastName = "Candidate3",
                Email = "Candidate3",
                CallTime = "Candidate3",
                Comment = "Candidate3",
                GitHub = "Candidate3",
                LinkedIn = "Candidate3",
                PhoneNumber = "Candidate3"
            };
            var result = service.AddOrUpdateCandidateContact(newCandidateContact).Result;
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            
            var candidateContact = _context.CandidateContacts.Find(result.Id);
            compare(result, candidateContact);





        }
        [Fact]
        public void CandidateContactService__Update__success()
        {
            CandidateContactService service = PrepareService();
            Assert.NotNull(service);
            var newCandidateContact = new Task.Business.DTO.CandidateContactDTO
            {
                Id= 1,
                FirstName = "updateCandidate1",
                LastName = "updateCandidate1",
                Email = "updateCandidate1",
                CallTime = "updateCandidate1",
                    Comment = "updateCandidate1",
                    GitHub = "updateCandidate1",
                    LinkedIn = "updateCandidate1",
                    PhoneNumber = "updateCandidate1"
                    
            };
            var result = service.AddOrUpdateCandidateContact(newCandidateContact).Result;
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
           
            var candidateContact = _context.CandidateContacts.Find(result.Id);
           compare(result, candidateContact);
               

        }
        [Fact]
        public void CandidateContactService__GetAll__success()
        {
            CandidateContactService service = PrepareService();
            Assert.NotNull(service);
            var result = service.GetAllCandidateContacts().Result;
            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
           
            var candidateContacts = _context.CandidateContacts;
            Assert.NotNull(candidateContacts);
            Assert.Equal(result.Count(), candidateContacts.Count());
            foreach (var candidateContact in candidateContacts)
            {
                var candidateContactDTO = result.FirstOrDefault(e => e.Id == candidateContact.Id);
                compare(candidateContactDTO, candidateContact);
            }
        }
        [Fact]
        public void CandidateContactService__GetAll__TestCache()
        {
            CandidateContactService service = PrepareService();
            Assert.NotNull(service);
            //First Add 1000 CandidateContacts
            List<long> ids = new List<long>();
            for (int i = 0; i < 2000; i++)
            {
                var newCandidateContact = new Task.Business.DTO.CandidateContactDTO
                {
                    FirstName = $"Candidate{i}",
                    LastName = $"Candidate{i}",
                    Email = $"Candidate{i}",
                    CallTime = $"Candidate{i}",
                    Comment = $"Candidate{i}",
                    GitHub = $"Candidate{i}",
                    LinkedIn = $"Candidate{i}",
                    PhoneNumber = $"Candidate{i}"

                    
                };
                var result = service.AddOrUpdateCandidateContact(newCandidateContact).Result;
                Assert.NotNull(result);
                Assert.True(result.Id > 0);
                ids.Add(result.Id);
            }
            //Get All CandidateContacts without cache and mesure time
            var Timer = System.Diagnostics.Stopwatch.StartNew();
            var resultWithoutCache = service.GetAllCandidateContacts().Result;
            Timer.Stop();
            var timeWithoutCache = Timer.ElapsedMilliseconds;
            Assert.NotNull(resultWithoutCache);
            Assert.True(resultWithoutCache.Count() > 0);
            //Get All CandidateContacts with cache and mesure time
            Timer = System.Diagnostics.Stopwatch.StartNew();
            var resultWithCache = service.GetAllCandidateContacts().Result;
            Timer.Stop();
            var timeWithCache = Timer.ElapsedMilliseconds;
            Assert.NotNull(resultWithCache);
            Assert.True(resultWithCache.Count() > 0);
            //Check if time with cache is less than time without cache
            Assert.True(timeWithCache < timeWithoutCache);
            //Check if the result is the same
            Assert.Equal(resultWithoutCache.Count(), resultWithCache.Count());
            foreach (var candidateContact in resultWithoutCache)
            {
                var candidateContactDTO = resultWithCache.FirstOrDefault(e => e.Id == candidateContact.Id);
                compareDTOS(candidateContactDTO, candidateContact);
            }
            //Delete all CandidateContacts
            foreach (var id in ids)
            {
                var candidateContact = _context.CandidateContacts.Find(id);
                _context.CandidateContacts.Remove(candidateContact);
            }
            _context.SaveChanges();



        }

        [InlineData(null, "updateCandidate1", "updateCandidate1", "updateCandidate1")]
        [InlineData("updateCandidate1", null, "updateCandidate1", "updateCandidate1")]
        [InlineData("updateCandidate1", "updateCandidate1", null, "updateCandidate1")]
        [InlineData("updateCandidate1", "updateCandidate1", "updateCandidate1", null)]
        [InlineData("", "updateCandidate1", "updateCandidate1", "updateCandidate1")]
        [InlineData("updateCandidate1", "", "updateCandidate1", "updateCandidate1")]
        [InlineData("updateCandidate1", "updateCandidate1", "", "updateCandidate1")]
        [InlineData("updateCandidate1", "updateCandidate1", "updateCandidate1", "")]
        [Theory]
        public void CandidateContactService__Update__fail(string firstName,string lastName,string comment,string email )
        {
            CandidateContactService service = PrepareService();
            Assert.NotNull(service);
            var newCandidateContact = new Task.Business.DTO.CandidateContactDTO
            {
                Id = 1,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                CallTime = "updateCandidate1",
                Comment = comment,
                GitHub = "updateCandidate1",
                LinkedIn = "updateCandidate1",
                PhoneNumber = "updateCandidate1"

            };
            try
            {
                var result = service.AddOrUpdateCandidateContact(newCandidateContact).Result;
                Assert.Fail("CandidateContact Updated!!!");

            }
            catch (System.AggregateException ex)
            {
                Assert.NotNull(ex);
                Assert.NotNull(ex.InnerException);
                Assert.NotNull(ex.InnerException.Message);
                Assert.Contains("Invalid Candidate Contact", ex.InnerException.Message);
            }

        }
        [InlineData(null, "updateCandidate1", "updateCandidate1", "updateCandidate1")]
        [InlineData("updateCandidate1", null, "updateCandidate1", "updateCandidate1")]
        [InlineData("updateCandidate1", "updateCandidate1", null, "updateCandidate1")]
        [InlineData("updateCandidate1", "updateCandidate1", "updateCandidate1", null)]
        [InlineData("", "updateCandidate1", "updateCandidate1", "updateCandidate1")]
        [InlineData("updateCandidate1", "", "updateCandidate1", "updateCandidate1")]
        [InlineData("updateCandidate1", "updateCandidate1", "", "updateCandidate1")]
        [InlineData("updateCandidate1", "updateCandidate1", "updateCandidate1", "")]
        [Theory]
        public void CandidateContactService__Create__fail(string firstName, string lastName, string comment, string email)
        {
            CandidateContactService service = PrepareService();
            Assert.NotNull(service);
            var newCandidateContact = new Task.Business.DTO.CandidateContactDTO
            {
                Id = 0,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                CallTime = "updateCandidate1",
                Comment = comment,
                GitHub = "updateCandidate1",
                LinkedIn = "updateCandidate1",
                PhoneNumber = "updateCandidate1"

            };
            try
            {
                var result = service.AddOrUpdateCandidateContact(newCandidateContact).Result;
                Assert.Fail("CandidateContact created!!!");
            }
            catch (System.AggregateException ex)
            {
                Assert.NotNull(ex);
                Assert.NotNull(ex.InnerException);
                Assert.NotNull(ex.InnerException.Message);
                Assert.Contains("Invalid Candidate Contact", ex.InnerException.Message);
            }

        }

    }
}
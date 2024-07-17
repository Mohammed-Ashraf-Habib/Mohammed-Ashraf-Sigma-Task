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
            CandidateContactService service = new CandidateContactService(candidateContactRepository, unitOfWorkAsync, mapper);
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
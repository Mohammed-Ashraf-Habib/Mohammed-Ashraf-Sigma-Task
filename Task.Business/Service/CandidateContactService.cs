using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Business.DTO;
using Task.Business.IService;
using Task.DAL.IRepositories;

namespace Task.Business.Service
{
    public class CandidateContactService: ICandidateContactService
    {
        private readonly ICandidateContactRepository _candidateContactRepository;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IMapper _mapper;
        
        public CandidateContactService(ICandidateContactRepository candidateContactRepository, IUnitOfWorkAsync unitOfWorkAsync,IMapper mapper)
        {
            _candidateContactRepository = candidateContactRepository;
            _unitOfWorkAsync = unitOfWorkAsync;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CandidateContactDTO>> GetAllCandidateContacts()
        {
            var candidateContacts = await _candidateContactRepository.GetAsync(null);
            return candidateContacts.Select(e => e.ToDTO(_mapper));
        }

        public async Task<CandidateContactDTO> AddOrUpdateCandidateContact(CandidateContactDTO candidateContact)
        {
            if (!ValidateCandidateContact(candidateContact))
            {
                throw new Exception("Invalid Candidate Contact");
            }
            var candidateContactEntity = candidateContact.ToEntity(_mapper);
            if (candidateContactEntity.Id == 0)
            {
                await _candidateContactRepository.AddAsync(candidateContactEntity);
            }
            else
            {
                await _candidateContactRepository.UpdateAsync(candidateContactEntity);
            }
            await _unitOfWorkAsync.CommitAsync();
            return candidateContactEntity.ToDTO(_mapper);
        }
        private bool ValidateCandidateContact(CandidateContactDTO candidateContact)
        {
            if (string.IsNullOrEmpty(candidateContact.FirstName) || string.IsNullOrEmpty(candidateContact.LastName) || string.IsNullOrEmpty(candidateContact.Email) || string.IsNullOrEmpty(candidateContact.Comment))
            {
                return false;
            }
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Business.DTO;

namespace Task.Business.IService
{
    public interface ICandidateContactService
    {
        Task<IEnumerable<CandidateContactDTO>> GetAllCandidateContacts();
        Task<CandidateContactDTO> AddOrUpdateCandidateContact(CandidateContactDTO candidateContact);
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Task.Business.DTO;
using Task.Business.IService;

namespace Task.WepApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidateContactController : ControllerBase
    {
       
        private readonly ICandidateContactService _candidateContactService;
        public CandidateContactController(ICandidateContactService candidateContactService)
        {
            _candidateContactService = candidateContactService;
        }

        [HttpGet]
        [Route("GetAllCandidateContacts")]
        public async Task<IEnumerable<CandidateContactDTO>> GetAllCandidateContacts()
        {
            return await _candidateContactService.GetAllCandidateContacts();
        }
        [HttpPost]
        [Route("AddOrUpdateCandidateContact")]
        public async Task<CandidateContactDTO> AddOrUpdateCandidateContact(CandidateContactDTO candidateContact)
        {
            return await _candidateContactService.AddOrUpdateCandidateContact(candidateContact);
        }


      
    }
}

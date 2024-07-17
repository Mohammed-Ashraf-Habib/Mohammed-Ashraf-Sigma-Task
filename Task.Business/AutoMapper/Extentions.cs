using AutoMapper;
using Task.Business.DTO;
using Task.DAL.Entity;





namespace Task.Business
{

    public static class Extentions
    {

        public static CandidateContact ToEntity(this CandidateContactDTO CandidateContactDTO, IMapper mapper)
        {
            return mapper.Map<CandidateContact>(CandidateContactDTO);
        }
        public static CandidateContactDTO ToDTO(this CandidateContact CandidateContact, IMapper mapper)
        {
            return mapper.Map<CandidateContactDTO>(CandidateContact);
        }


    }
}

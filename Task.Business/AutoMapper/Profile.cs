using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Task.Business.DTO;
using Task.DAL.Entity;




namespace Task.Business
{

    public class Profile : AutoMapper.Profile
    {
  


 
        public Profile()
        {
          
           CreateMap<CandidateContactDTO, CandidateContact>().ReverseMap();






        }

    }
}

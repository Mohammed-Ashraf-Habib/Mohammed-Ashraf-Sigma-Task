using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.DAL.Entity
{
    public class CandidateContact
    {
        
        public long Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public string CallTime { get; set; }
        public string LinkedIn { get; set; }
        public string GitHub { get; set; }
        [Required]
        public string Comment { get; set; }

    }
}

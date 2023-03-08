using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DotNet.ApplicationCore.DTOs
{
    public class UserModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        
        public string Password { get; set; }

        public string Grant_type { get; set; }
        //public string Username { get; set; }
        //public string Password { get; set; }
        //public string CompanyId { get; set; }
        //public string Refreshtoken { get; set; }
        //public string Ip { get; set; }
    }
}

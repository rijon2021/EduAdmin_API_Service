using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.ApplicationCore.DTOs
{
    public class UserDto
    {
       
        public UserDto(string UserName, string Password)
        {
            this.UserName = UserName;
            this.Password = Password;
        }

        public string UserName { get; set; }
        public int OrginzationId { get; set; }
        public string Password { get; set; }
    }
}

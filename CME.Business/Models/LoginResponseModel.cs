using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class LoginResponseModel
    {
        public UserViewModel User {get;set;}
        public string Token { get; set; }
    }
}

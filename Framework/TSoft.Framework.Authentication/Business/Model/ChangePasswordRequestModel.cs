using System;
using System.Collections.Generic;
using System.Text;

namespace TSoft.Framework.Authentication.Business.Model
{
   public class ChangePasswordRequestModel
    {
        public string Password { get; set; }
        public string PasswordNew { get; set; }
       
    }
}

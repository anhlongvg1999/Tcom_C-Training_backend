using System;
using System.Collections.Generic;
using System.Text;

namespace TSoft.Framework.Authentication.Business.Model
{
   public class CheckCodeViewModel
    {
        public Guid UserId { get; set; }
        public string VerificationCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class ChangePasswordRequestModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}

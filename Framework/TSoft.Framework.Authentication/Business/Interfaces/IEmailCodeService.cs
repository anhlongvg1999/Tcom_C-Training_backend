using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.Authentication.Business.Model;

namespace TSoft.Framework.Authentication.Business.Interfaces
{
   public interface IEmailCodeService
    {
        void SendMail(string addressEmail, string verificationCode);
    }
}

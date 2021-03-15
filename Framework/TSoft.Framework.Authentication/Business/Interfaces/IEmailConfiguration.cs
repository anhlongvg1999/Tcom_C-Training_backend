using System;
using System.Collections.Generic;
using System.Text;

namespace TSoft.Framework.Authentication.Business.Interfaces
{
   public interface IEmailConfiguration
    {
        bool Enable { get; set; }
        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }

    }
}

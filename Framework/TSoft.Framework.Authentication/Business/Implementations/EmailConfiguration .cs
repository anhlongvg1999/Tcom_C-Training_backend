using System;
using System.Collections.Generic;
using System.Text;
using TSoft.Framework.Authentication.Business.Interfaces;

namespace TSoft.Framework.Authentication.Business.Implementations
{
   public class EmailConfiguration : IEmailConfiguration
    {
        public bool Enable { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
    }
}

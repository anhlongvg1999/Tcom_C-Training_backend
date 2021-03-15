using System;
using System.Collections.Generic;
using System.Text;

namespace TSoft.Framework.Authentication.EmailSender.Configuration
{
   public class EmailSenderConfiguration
    {
        public bool Enable { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
    }
}

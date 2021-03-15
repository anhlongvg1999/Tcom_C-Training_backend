using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.Authentication.Business.Implementations;
using TSoft.Framework.Authentication.Business.Interfaces;
using TSoft.Framework.Authentication.Business.Model;

namespace TSoft.Framework.Authentication.EmailSender
{
   public class EmailSenderService : IEmailCodeService
    {
        #region Contructor
        private readonly DataContextBase _dataContext;
        private readonly IEmailConfiguration _emailConfiguration;
        public EmailSenderService(IEmailConfiguration emailConfiguration,
             DataContextBase dataContext)
        {
            _emailConfiguration = emailConfiguration;
            _dataContext = dataContext;
        }
        #endregion Contructor

        public async void SendMail(string addressEmail,string verificationCode)
        {
            //to address    
            string toadresstitle = "Test Send Email";
            var mimemessage = new MimeMessage();

            mimemessage.From.Add(new MailboxAddress(toadresstitle, _emailConfiguration.SmtpUsername));
            mimemessage.To.Add(new MailboxAddress(toadresstitle, addressEmail));
            mimemessage.Subject = verificationCode;
            using (var client = new SmtpClient())
            {
                client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, false);
                client.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                await client.SendAsync(mimemessage);
                await client.DisconnectAsync(true);
            }
          
        }
    }
}

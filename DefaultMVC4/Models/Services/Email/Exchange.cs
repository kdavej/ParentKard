using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Microsoft.Exchange.WebServices.Data;

namespace DefaultMVC4.Models.Services.Email
{
    public class Exchange
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ServiceURL { get; set; }

        public void SendMessage(string Message, string To, string From, string Subject)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP2);
            //service.AutodiscoverUrl("youremailaddress@yourdomain.com");

            service.Url = new Uri(ServiceURL);

            //service.UseDefaultCredentials = true;
            service.Credentials = new NetworkCredential(UserName, Password);
            //service.Credentials = new WebCredentials("username", "password");

            EmailMessage message = new EmailMessage(service);
            message.Subject = Subject;
            message.Body = Message;
            message.ToRecipients.Add(To);
            message.From = From;
            message.Send();

        }   
    }
}
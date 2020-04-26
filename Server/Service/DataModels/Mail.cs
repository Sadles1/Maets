using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Mail
    {
        private MailMessage message;
        private MailAddress from = new MailAddress("maetsofficial@gmail.com", "Maets");

        public Mail(string mail)
        {
            MailAddress to = new MailAddress(mail);
            message = new MailMessage(from,to);
            message.IsBodyHtml = true;
        }

        public string Head
        {
            set { message.Subject = value; }
        }

        public string Body
        {
            set { message.Body = value; }
        }
        
        public void SendMsg()
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("maetsofficial@gmail.com", "Zxcv1234zxcv12345"),
                EnableSsl = true
            };
            smtp.Send(message);
        }
    }
}

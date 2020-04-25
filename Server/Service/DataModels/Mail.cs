using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Mail
    {
        public string Test;

//         private DataProvider dp = new DataProvider();
//         private MailMessage message;
// 
//         
//         public Mail(string mail)
//         {
//             MailAddress from = new MailAddress("maetsofficial@gmail.com", "Maets");
//             MailAddress to = new MailAddress(mail);
//             message = new MailMessage(from, to);
//             message.IsBodyHtml = true;
//         }
//         public string Head
//         {
//             set { message.Subject = value; }
//         }
//         public string Body
//         {
//             set { message.Body = value; }
//         }



        //         public string ResetPassword(string Mail)
        //         {
        //             string Code = dp.GenRandomString("QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890", 4);
        //             MailAddress from = new MailAddress("maetsofficial@gmail.com", "Maets");
        //             MailAddress to = new MailAddress(Mail);
        //             MailMessage message = new MailMessage(from, to);
        //             message.Subject = "Смена пароля Maets";
        //             // текст письма
        //             message.Body = $"<html><head></head><body><p><center> Доброго времени суток!</center></p><p>Если вы видите это письмо, значит происходит смена пароля Вашего аккаунта на Maets.</p><p> Ваш код подтверждения: </p><p></p><h2><center>{Code}</center></h2><p> Если вы не ожидали получить это письмо, то просто игнорируйте его.</p><p></p><p> С уважением, команда Maets</p><p></p></body></html>";
        //             message.IsBodyHtml = true;
        //             SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
        //             {
        //                 Credentials = new NetworkCredential("maetsofficial@gmail.com", "Zxcv1234zxcv12345"),
        //                 EnableSsl = true
        //             };
        //             smtp.Send(message);
        //             return Code;
        //         }
    }
}

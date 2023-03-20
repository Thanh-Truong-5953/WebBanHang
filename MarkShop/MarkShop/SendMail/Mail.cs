using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MarkShop.NewFolder1
{
    public class Mail
    {
        public  void SendMail (string EmailNhan, string ChuDe, string NoiDung)
        {
            var EmailGui = ConfigurationManager.AppSettings["EmailGui"].ToString();
            var ChuDeEmail = ConfigurationManager.AppSettings["ChuDeEmail"].ToString();
            var MatKhauEmail = ConfigurationManager.AppSettings["MatKhauEmail"].ToString();
            var SMTPHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var SMTPPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
            bool SSLBat = bool.Parse(ConfigurationManager.AppSettings["SSLBat"].ToString());

            MailMessage mess = new MailMessage(EmailGui,EmailNhan);
            mess.Subject = ChuDe;
            mess.IsBodyHtml = true;
            mess.Body = NoiDung;
            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(EmailGui, MatKhauEmail);
            client.Host = SMTPHost;
            client.EnableSsl= SSLBat;
            client.Port = !string.IsNullOrEmpty(SMTPPort) ? Convert.ToInt32(SMTPPort) : 0;
            client.Send(mess);

        }
        
    }
}
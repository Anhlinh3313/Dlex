using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Core.Business.Core.Utils
{
    public static class MailUtil
    {
        public static string SendEmail(string _MailTo, string _Title, string _Body, string _Name)
        {
            return "FALSE";
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = false;
            client.Host = "mail.vietstarexpress.com";
            var credentials = new System.Net.NetworkCredential("system@vietstarexpress.com", "CRYtYU[gR01X");
            client.UseDefaultCredentials = true;
            client.Credentials = credentials;
            MailMessage mess = new MailMessage();
            mess.From = new MailAddress("system@vietstarexpress.com", _Name);
            mess.To.Add(_MailTo);
            mess.Subject = _Title;
            mess.IsBodyHtml = true;
            mess.Body = _Body;
            try
            {
                client.Send(mess);
                return "TRUE";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }
    }
}

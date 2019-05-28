using System;
using System.Net.Mail;

namespace AquaparkApplication.Other
{
    public static class Email
    {
        public static bool SendEmail(string email, string bodyMessage, string bodySubject)
        {
            try
            {
                MailMessage mail = new MailMessage("jakiekieszonkowe@gmail.com", email);
                SmtpClient client = new SmtpClient();
                client.EnableSsl = true;
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("aquaparksupersystem@gmail.com", "asdzxcasd");
                client.Host = "smtp.gmail.com";
                mail.Subject = bodySubject;
                mail.Body = bodyMessage;
                client.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}

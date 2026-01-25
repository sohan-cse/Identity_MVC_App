using System.Net.Mail;

namespace Identity_MVC_App.Helper
{
    public class EmailSender
    {
        public bool SendMail(string to,string subject,string body)
        {
            MailMessage message=new MailMessage();
            SmtpClient smtpClient =new SmtpClient();
            message.From = new MailAddress("sohan.pust.14@gmail.com");
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("sohan.pust.14@gmail.com", "dxtm tzfm vvli iouj"); // Use App Password here
            smtpClient.DeliveryMethod= SmtpDeliveryMethod.Network;

            try
            {
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }
    }
}

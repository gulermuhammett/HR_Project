using System.Net.Mail;
using System.Net;

namespace HR_Project.UI.Areas.CompanyManager.Models
{
    public static class EmailService
    {

        public static void SendEmail(string recipientEmail, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("atakankelle95@hotmail.com");
            mail.To.Add(recipientEmail);
            mail.Subject = subject;
            mail.Body = body;

            SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("atakankelle95@hotmail.com", "147852369aA.");
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                // Hata yönetimi işlemlerini burada gerçekleştirin.
                Console.WriteLine("E-posta gönderimi sırasında bir hata oluştu:");
                Console.WriteLine(ex.ToString());
            }
        }

    }
}

using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;


namespace Hospital_Project.Entities.UserDTOs.EmailSender
{
        public class EmailSender : IEmailSender
        {
            public async Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                var fromEmail = "hospitalsystem00@gmail.com";
                var fromName = " مستشفى الخير";
                var fromPassword = "zwfajvxyckybjdpw"; 

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, fromPassword),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);

                mailMessage.Dispose();
                smtpClient.Dispose();
            }
        }
    }



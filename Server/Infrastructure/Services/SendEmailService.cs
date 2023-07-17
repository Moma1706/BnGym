using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Policy;
using Application.Common.Interfaces;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services
{
    public class SendEmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly IConfiguration _configuration;

        public SendEmailService(ISendGridClient sendGridClient, IConfiguration configuration)
        {
            _sendGridClient = sendGridClient;
            _configuration = configuration;
        }

        public async Task SendConfirmationEmailAsync(string email)
        {
            //*******************************SNMP SERVER FOR SENDING EMAIL*********************************************//
            //var url = _configuration["SendGrid:EmailConfirmationUrl"].Replace("*email*", email).Replace("*token*", token);

            //var sendGridMessage = new SendGridMessage();
            //sendGridMessage.SetFrom(_configuration["SendGrid:From"], _configuration["SendGrid:FromDisplayName"]);
            //sendGridMessage.AddTo(email);
            //sendGridMessage.SetSubject("MyNotes Email Confirmation");
            //sendGridMessage.SetTemplateId(_configuration["SendGrid:EmailConfirmationTemplateId"]);
            //sendGridMessage.SetTemplateData(new { confirmationUrl = url });

            //var emailResponse = await _sendGridClient.SendEmailAsync(sendGridMessage);

            //if (!emailResponse.IsSuccessStatusCode)
            //{
            //    var message = await emailResponse.Body.ReadAsStringAsync();
            //}

            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine("Email Confirmation Message");
            System.Diagnostics.Debug.WriteLine("--------------------------");
            System.Diagnostics.Debug.WriteLine($"TO: {email}");
        }

        public async Task SendResetPasswordAsync(string email, string token)
        {
            // var url = _configuration["SendGrid:PasswordResetUrl"].Replace("*email*", email).Replace("*token*", token);

            //var sendGridMessage = new SendGridMessage();
            //sendGridMessage.SetFrom(_configuration["SendGrid:From"], _configuration["SendGrid:FromDisplayName"]);
            //sendGridMessage.AddTo(email);
            //sendGridMessage.SetSubject("MyNotes Password Reset");
            //sendGridMessage.SetTemplateId(_configuration["SendGrid:PasswordResetTemplateId"]);
            //sendGridMessage.SetTemplateData(new { passwordResetUrl = url });

            //var emailResponse = await _sendGridClient.SendEmailAsync(sendGridMessage);
            //if (!emailResponse.IsSuccessStatusCode)
            //{
            //    var message = await emailResponse.Body.ReadAsStringAsync();
            //}
            var url = _configuration["SendGrid:PasswordResetUrl"].Replace("*email*", email).Replace("*token*", token);

            string mailFrom = "testmilanmilica@​yahoo.com";
            var lnkHref = "<a href='" + new UriBuilder(url) + "'>Reset Password</a>";
            string subject = "Your changed password";
            string body = "<b>Please find the Password Reset Link. </b><br/>" + lnkHref;
            var mailMessage = new MailMessage();
            var fromAddress = new MailAddress(mailFrom, "BN gym");
            mailMessage.From = fromAddress;
            mailMessage.To.Add(email);
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;
            var smtpClient = new SmtpClient();
            smtpClient.Host = "localhost";
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            smtpClient.PickupDirectoryLocation = "C:\\Mail";
            smtpClient.Send(mailMessage);
        }

        public async Task SendEmailSubscriptionexpires(string email, string token)
        {
            ////var url = _configuration["SendGrid:PasswordResetUrl"].Replace("*email*", email).Replace("*token*", token);

            //string mailFrom = "milan.momcilovic582@​gmail.com";
            ////var lnkHref = "<a href='" + new UriBuilder(url) + "'>Reset Password</a>";
            //string subject = "Clanarina istice!";
            //string body = "<b>Vasa clanarina istice za 2 dana, molim vas produzite clanarinu! </b><br/>";
            //var mailMessage = new MailMessage();
            //var fromAddress = new MailAddress(mailFrom, "BN gym");
            //mailMessage.From = fromAddress;
            //mailMessage.To.Add("milica.simeunovic97@gmail.com");
            //mailMessage.Body = body;
            //mailMessage.IsBodyHtml = true;
            //mailMessage.Subject = subject;
            //var smtpClient = new SmtpClient();
            //smtpClient.Host = "localhost";
            //smtpClient.Port = 162;
            //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            ////smtpClient.PickupDirectoryLocation = "C:\\Mail";
            //smtpClient.Send(mailMessage);

            SendEmail("tamara");
        }

        public void SendEmail(string body)
        {
            var initializer = new BaseClientService.Initializer();
            initializer.ApiKey = "AIzaSyAjYzeRuYMlRNAOz3t3kcHbpLRCMUEt1zE";
            var service = new GmailService(initializer);

            var mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress("bngympulse@gmail.com");
            mailMessage.To.Add("milan.momcilovic582@gmail.com\r\n");
            mailMessage.Subject = "Redfish Sites Available";
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = false;
            mailMessage.Headers.Add("client_id", "158843384065-b0tjbc6kab297vnpi7tfgmthagn2iei4.apps.googleusercontent.com");
            mailMessage.Headers.Add("client_secret", "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");

            var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mailMessage);

            var gmailMessage = new Google.Apis.Gmail.v1.Data.Message
            {
                Raw = Encode(mimeMessage)
            };

            Google.Apis.Gmail.v1.UsersResource.MessagesResource.SendRequest request = service.Users.Messages.Send(gmailMessage, "bngympulse");

            request.Execute();
        }

        public static string Encode(MimeMessage mimeMessage)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                mimeMessage.WriteTo(ms);
                return Convert.ToBase64String(ms.GetBuffer())
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
            }
        }
    }
}
using System.Net.Mail;
using System.Security.Policy;
using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
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
    }
}
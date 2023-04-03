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

        public async Task SendConfirmationEmailAsync(string email, string token)
        {
            //*******************************SNMP SERVER FOR SENDING EMAIL*********************************************//
            /*var url = _configuration["SendGrid:EmailConfirmationUrl"].Replace("*email*", email).Replace("*token*", token);

            var sendGridMessage = new SendGridMessage();
            sendGridMessage.SetFrom(_configuration["SendGrid:From"], _configuration["SendGrid:FromDisplayName"]);
            sendGridMessage.AddTo(email);
            sendGridMessage.SetSubject("MyNotes Email Confirmation");
            sendGridMessage.SetTemplateId(_configuration["SendGrid:EmailConfirmationTemplateId"]);
            sendGridMessage.SetTemplateData(new { confirmationUrl = url });

            await _sendGridClient.SendEmailAsync(sendGridMessage);

            if (!emailResponse.IsSuccessStatusCode)
            {
                var message = await emailResponse.Body.ReadAsStringAsync();
            }*/

            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine("Email Confirmation Message");
            System.Diagnostics.Debug.WriteLine("--------------------------");
            System.Diagnostics.Debug.WriteLine($"TO: {email}");
            System.Diagnostics.Debug.WriteLine($"TOKEN: {token}");
        }

        public async Task SendResetPasswordAsync(string email, string token)
        {
            /*
             var url = _configuration["SendGrid:PasswordResetUrl"].Replace("*email*", email).Replace("*token*", token);

             var sendGridMessage = new SendGridMessage();
             sendGridMessage.SetFrom(_configuration["SendGrid:From"], _configuration["SendGrid:FromDisplayName"]);
             sendGridMessage.AddTo(email);
             sendGridMessage.SetSubject("MyNotes Password Reset");
             sendGridMessage.SetTemplateId(_configuration["SendGrid:PasswordResetTemplateId"]);
             sendGridMessage.SetTemplateData(new { passwordResetUrl = url });

             var emailResponse = await _sendGridClient.SendEmailAsync(sendGridMessage);
            */

            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine("Reset Password Message");
            System.Diagnostics.Debug.WriteLine("--------------------------");
            System.Diagnostics.Debug.WriteLine($"TO: {email}");
            System.Diagnostics.Debug.WriteLine($"TOKEN: {token}");
        }
    }
}

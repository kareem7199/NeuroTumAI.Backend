using Microsoft.Extensions.Configuration;
using NeuroTumAI.Core.Services.Contract;
using System.Net;
using System.Net.Mail;

namespace NeuroTumAI.Service.Services.EmailService
{
	public class EmailService : IEmailService
	{
		private readonly IConfiguration _configuration;

		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendAsync(string recipients, string subject, string body)
		{

			var senderEmail = _configuration["EmailSettings:SenderEmail"];
			var senderPassword = _configuration["EmailSettings:SenderPassword"];

			var emailMessage = new MailMessage();

			emailMessage.From = new MailAddress(senderEmail!);
			emailMessage.To.Add(recipients);
			emailMessage.Subject = subject;
			emailMessage.Body = $"<html> <body> {body} </body> </html>";
			emailMessage.IsBodyHtml = true;

			var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpClientServer"], int.Parse(_configuration["EmailSettings:SmtpClientPort"]))
			{
				Credentials = new NetworkCredential(senderEmail, senderPassword),
				EnableSsl = true
			};

			await smtpClient.SendMailAsync(emailMessage);
		}
	}
}

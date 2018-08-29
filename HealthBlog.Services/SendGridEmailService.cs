namespace HealthBlog.Services
{
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Identity.UI.Services;
	using SendGrid;
	using SendGrid.Helpers.Mail;

	public class SendGridEmailService : IEmailSender
	{
		private readonly string apiKey;

		public SendGridEmailService(string apiKey)
		{
			this.apiKey = apiKey;
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var apiKey = this.apiKey;
			var client = new SendGridClient(apiKey);
			var from = new EmailAddress("bleedaxe@gmail.com");
			var to = new EmailAddress(email);
			var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlMessage, htmlMessage);
			var response = await client.SendEmailAsync(msg);
		}
	}
}

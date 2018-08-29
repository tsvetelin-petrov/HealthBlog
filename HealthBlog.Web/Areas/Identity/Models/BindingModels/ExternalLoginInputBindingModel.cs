namespace HealthBlog.Web.Areas.Identity.Models.BindingModels
{
	using System.ComponentModel.DataAnnotations;

	public class ExternalLoginInputBindingModel
	{
		[Required]
		public string Username { get; set; }

		[Required]
		[Display(Name = "Full Name")]
		public string FullName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}

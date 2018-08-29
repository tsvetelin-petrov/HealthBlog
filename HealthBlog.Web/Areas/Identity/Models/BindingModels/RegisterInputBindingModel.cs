namespace HealthBlog.Web.Areas.Identity.Models.BindingModels
{
	using System.ComponentModel.DataAnnotations;
	public class RegisterInputBindingModel
	{
		[Required]
		[Display(Name = "Потребитеско име")]
		public string Username { get; set; }

		[Required]
		[Display(Name = "Име")]
		public string FullName { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
		[DataType(DataType.Password)]
		[Display(Name = "Парола")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Повтори паролата")]
		[Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
		public string ConfirmPassword { get; set; }

		[Display(Name = "Треньор ли си?")]
		public bool AreYouTrainer { get; set; }
	}
}

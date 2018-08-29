namespace HealthBlog.Common.Users.BindingModels
{
	using System.ComponentModel.DataAnnotations;

	public class MealCreateBindingModel
    {
		[Required]
		[Display(Name = "Име")]
		public string Name { get; set; }

		[Display(Name = "Описание")]
		public string Description { get; set; }
	}
}

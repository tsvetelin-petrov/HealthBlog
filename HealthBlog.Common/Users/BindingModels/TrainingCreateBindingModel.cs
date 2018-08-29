namespace HealthBlog.Common.Users.BindingModels
{
	using System.ComponentModel.DataAnnotations;

	public class TrainingCreateBindingModel
	{
		[Required]
		[Display(Name = "Име")]
		public string Name { get; set; }

		[Display(Name = "Тип")]
		public string Type { get; set; }

		[Display(Name = "Описание")]
		public string Description { get; set; }
	}
}

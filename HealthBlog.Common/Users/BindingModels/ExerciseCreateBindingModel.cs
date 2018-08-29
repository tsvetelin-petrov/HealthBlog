namespace HealthBlog.Common.Users.BindingModels
{
	using System.ComponentModel.DataAnnotations;
	public class ExerciseCreateBindingModel
    {
		[Required]
		[Display(Name = "Име")]
		public string Name { get; set; }

		[Display(Name = "Мускулна група")]
		public string TargetMuscle { get; set; }

		[Display(Name = "Описание")]
		public string Description { get; set; }
	}
}

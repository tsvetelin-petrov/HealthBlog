namespace HealthBlog.Common.Users.BindingModels
{
	using System.ComponentModel.DataAnnotations;

	using Constants;

	public class ExerciseCreateBindingModel
    {
		[Required]
		[StringLength(30, MinimumLength = 2)]
		[Display(Name = AttributeDisplayNameConstants.Name)]
		public string Name { get; set; }

		[Required]
		[StringLength(30, MinimumLength = 2)]
		[Display(Name = AttributeDisplayNameConstants.TargetMuscle)]
		public string TargetMuscle { get; set; }

		[Required]
		[StringLength(30, MinimumLength = 2)]
		[Display(Name = AttributeDisplayNameConstants.Description)]
		public string Description { get; set; }
	}
}

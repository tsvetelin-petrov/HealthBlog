namespace HealthBlog.Common.Users.BindingModels
{
	using System.ComponentModel.DataAnnotations;

	using Constants;

	public class ExerciseCreateBindingModel
    {
		[Required]
		[StringLength(ModelsLengthConstants.NameMaxLength, MinimumLength = ModelsLengthConstants.NameMinLength)]
		[Display(Name = AttributeDisplayNameConstants.Name)]
		public string Name { get; set; }

		[Required]
		[StringLength(ModelsLengthConstants.MuscleMaxLength, MinimumLength = ModelsLengthConstants.MuscleMinLength)]
		[Display(Name = AttributeDisplayNameConstants.TargetMuscle)]
		public string TargetMuscle { get; set; }

		[Required]
		[StringLength(ModelsLengthConstants.DescriptionMaxLength, MinimumLength = ModelsLengthConstants.DescriptionMinLength)]
		[Display(Name = AttributeDisplayNameConstants.Description)]
		public string Description { get; set; }
	}
}

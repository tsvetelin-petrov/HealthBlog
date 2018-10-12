namespace HealthBlog.Common.Users.BindingModels
{
	using System.ComponentModel.DataAnnotations;

	using Constants;

	public class TrainingCreateBindingModel
	{
		[Required]
		[StringLength(ModelsLengthConstants.NameMaxLength, MinimumLength = ModelsLengthConstants.NameMinLength)]
		[Display(Name = AttributeDisplayNameConstants.Name)]
		public string Name { get; set; }

		[Required]
		[StringLength(ModelsLengthConstants.TypeMaxLength, MinimumLength = ModelsLengthConstants.TypeMinLength)]
		[Display(Name = AttributeDisplayNameConstants.Type)]
		public string Type { get; set; }

		[Required]
		[StringLength(ModelsLengthConstants.TrainingDescriptionMaxLength, MinimumLength = ModelsLengthConstants.DescriptionMinLength)]
		[Display(Name = AttributeDisplayNameConstants.Description)]
		public string Description { get; set; }
	}
}

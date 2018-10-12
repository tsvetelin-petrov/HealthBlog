using HealthBlog.Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthBlog.Models
{
	public class Exercise
	{
		public int Id { get; set; }

		[Required]
		[StringLength(ModelsLengthConstants.NameMaxLength, MinimumLength = ModelsLengthConstants.NameMinLength)]
		public string Name { get; set; }

		[Required]
		[StringLength(ModelsLengthConstants.MuscleMaxLength, MinimumLength = ModelsLengthConstants.MuscleMinLength)]
		public string TargetMuscle { get; set; }

		[Required]
		[StringLength(ModelsLengthConstants.DescriptionMaxLength, MinimumLength = ModelsLengthConstants.DescriptionMinLength)]
		public string Description { get; set; }

		public string UserId { get; set; }
		public User User { get; set; }

		public ICollection<TrainingExercise> Trainings { get; set; }
	}
}
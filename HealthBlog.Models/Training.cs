namespace HealthBlog.Models
{
	using HealthBlog.Common.Constants;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class Training
    {
		public Training()
		{
			this.Days = new List<TrainingDay>();
			this.Exercises = new List<TrainingExercise>();
		}

		public int Id { get; set; }

		[Required]
		[StringLength(ModelsLengthConstants.NameMaxLength, MinimumLength = ModelsLengthConstants.NameMinLength)]
		public string Name { get; set; }

		[Required]
		[StringLength(ModelsLengthConstants.TypeMaxLength, MinimumLength = ModelsLengthConstants.TypeMinLength)]
		public string Type { get; set; }

		[Required]
		[StringLength(ModelsLengthConstants.TrainingDescriptionMaxLength, MinimumLength = ModelsLengthConstants.DescriptionMinLength)]
		public string Description { get; set; }

		public string UserId { get; set; }
		public User User { get; set; }

		public ICollection<TrainingExercise> Exercises { get; set; }

		public ICollection<TrainingDay> Days { get; set; }
	}
}

using HealthBlog.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace HealthBlog.Models
{
	public class TrainingExercise
	{
		public int TrainingId { get; set; }
		public Training Training { get; set; }

		public int ExerciseId { get; set; }
		public Exercise Exercise { get; set; }

		[Required]
		[Range(ModelsLengthConstants.SeriesMinCount, ModelsLengthConstants.SeriesMaxCount)]
		public int SeriesCount { get; set; }

		[Required]
		[Range(ModelsLengthConstants.RepetitionMinCount, ModelsLengthConstants.RepetitionMaxCount)]
		public int RepetitionCount { get; set; }
	}
}
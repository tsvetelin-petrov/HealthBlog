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
		[Range(1, 100)]
		public int SeriesCount { get; set; }

		[Required]
		[Range(1, 100)]
		public int RepetitionCount { get; set; }
	}
}
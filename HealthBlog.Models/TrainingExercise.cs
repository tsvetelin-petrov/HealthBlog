namespace HealthBlog.Models
{
	public class TrainingExercise
	{
		public int TrainingId { get; set; }
		public Training Training { get; set; }

		public int ExerciseId { get; set; }
		public Exercise Exercise { get; set; }

		public int SeriesCount { get; set; }

		public int RepetitionCount { get; set; }
	}
}
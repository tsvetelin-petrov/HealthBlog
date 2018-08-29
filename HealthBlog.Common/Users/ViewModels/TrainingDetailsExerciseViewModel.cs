namespace HealthBlog.Common.Users.ViewModels
{
	public class TrainingDetailsExerciseViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string TargetMuscle { get; set; }

		public int SeriesCount { get; set; }

		public int RepetitionCount { get; set; }
	}
}
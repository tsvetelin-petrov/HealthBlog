namespace HealthBlog.Common.Users.BindingModels
{
	using System.ComponentModel.DataAnnotations;
	public class TrainingExerciseInput
    {
		[Display(Name = "Упражнение")]
		public int ExerciseId { get; set; }

		[Display(Name = "Серии")]
		public int SeriesCount { get; set; }

		[Display(Name = "Повторения")]
		public int RepetitionCount { get; set; }
	}
}

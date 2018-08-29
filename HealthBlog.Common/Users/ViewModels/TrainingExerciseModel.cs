namespace HealthBlog.Common.Users.ViewModels
{
	using BindingModels;
	using System.Collections.Generic;

	public class TrainingExerciseModel
	{
		public string TrainingName { get; set; }

		public TrainingExerciseInput Input { get; set; }

		public IEnumerable<AllExercisesViewModel> Exercises { get; set; }
	}
}
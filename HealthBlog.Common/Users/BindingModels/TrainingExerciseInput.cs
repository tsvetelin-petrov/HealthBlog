namespace HealthBlog.Common.Users.BindingModels
{
	using System.ComponentModel.DataAnnotations;

	using Constants;

	public class TrainingExerciseInput
    {
		[Display(Name = AttributeDisplayNameConstants.Exercise)]
		public int ExerciseId { get; set; }

		[Display(Name = AttributeDisplayNameConstants.Series)]
		public int SeriesCount { get; set; }

		[Display(Name = AttributeDisplayNameConstants.Repeat)]
		public int RepetitionCount { get; set; }
	}
}

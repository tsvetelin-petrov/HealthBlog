namespace HealthBlog.Common.Users.ViewModels
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class TrainingDetailsViewModel
    {
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string Type { get; set; }

		public string Description { get; set; }

		public ICollection<TrainingDetailsExerciseViewModel> Exercises { get; set; }

		public bool IsCreatedByCurrentUser { get; set; }
	}
}

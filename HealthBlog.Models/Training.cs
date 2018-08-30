namespace HealthBlog.Models
{
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
		[StringLength(30, MinimumLength = 2)]
		public string Name { get; set; }

		[Required]
		[StringLength(30, MinimumLength = 2)]
		public string Type { get; set; }

		[Required]
		[StringLength(200, MinimumLength = 2)]
		public string Description { get; set; }

		public string UserId { get; set; }
		public User User { get; set; }

		public ICollection<TrainingExercise> Exercises { get; set; }

		public ICollection<TrainingDay> Days { get; set; }
	}
}

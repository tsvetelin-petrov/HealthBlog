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
		public string Name { get; set; }

		public string Type { get; set; }

		public string Description { get; set; }

		public string UserId { get; set; }
		public User User { get; set; }

		public ICollection<TrainingExercise> Exercises { get; set; }

		public ICollection<TrainingDay> Days { get; set; }
	}
}

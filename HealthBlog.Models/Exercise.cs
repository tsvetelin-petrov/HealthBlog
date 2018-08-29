using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthBlog.Models
{
	public class Exercise
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string TargetMuscle { get; set; }

		public string Description { get; set; }

		public string UserId { get; set; }
		public User User { get; set; }

		public ICollection<TrainingExercise> Trainings { get; set; }
	}
}
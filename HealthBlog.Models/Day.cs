namespace HealthBlog.Models
{
	using System.Collections.Generic;

	public class Day
	{
		public Day()
		{
			this.Meals = new List<MealDay>();
			this.Trainings = new List<TrainingDay>();
			this.Programs = new List<ProgramDay>();
		}

		public int Id { get; set; }

		public ICollection<ProgramDay> Programs { get; set; }

		public ICollection<MealDay> Meals { get; set; }

		public ICollection<TrainingDay> Trainings { get; set; }

		public string AuthorId { get; set; }
		public User Author { get; set; }
	}
}
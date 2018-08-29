namespace HealthBlog.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class Meal
	{
		public Meal()
		{
			this.Days = new List<MealDay>();
		}

		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

		public string UserId { get; set; }
		public User User { get; set; }

		public ICollection<MealDay> Days { get; set; }
	}
}
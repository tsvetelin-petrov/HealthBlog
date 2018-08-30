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
		[StringLength(30, MinimumLength = 2)]
		public string Name { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 2)]
		public string Description { get; set; }

		public string UserId { get; set; }
		public User User { get; set; }

		public ICollection<MealDay> Days { get; set; }
	}
}
namespace HealthBlog.Models
{
	using System;

	public class MealDay
	{
		public int MealId { get; set; }
		public Meal Meal { get; set; }

		public int DayId { get; set; }
		public Day Day { get; set; }

		public DateTime MealTime { get; set; }
	}
}
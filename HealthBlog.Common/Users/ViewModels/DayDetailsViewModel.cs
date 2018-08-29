namespace HealthBlog.Common.Users.ViewModels
{
	using System;
	using System.Collections.Generic;

	public class DayDetailsViewModel
    {
		public int Id { get; set; }

		public double WaterToDrink { get; set; }

		public IEnumerable<DayMealDetailsViewModel> Meals { get; set; }

		public IEnumerable<DayTrainingDetailsViewModel> Trainings { get; set; }

		public DateTime? Date { get; set; }

		public bool IsCreatedByCurrentUser { get; set; }
	}
}
